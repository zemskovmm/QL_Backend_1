using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using NickBuhro.Translit;
using QuartierLatin.Backend.Config;
using QuartierLatin.Backend.Database;
using QuartierLatin.Backend.Database.AppDbContextSeed;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Importer.DataModel;

namespace QuartierLatin.Backend.Cmdlets
{
    public class ImportCmdlet : CmdletBase<ImportCmdlet.ImportCmdletOptions>
    {
        private readonly AppDbContextManager _db;
        private readonly DatabaseConfig _dbConfig;

        [Verb("import")]
        public class ImportCmdletOptions
        {
            [Value(0)] public string Input { get; set; }
        }

        public ImportCmdlet(AppDbContextManager db, IConfiguration config)
        {
            _db = db;
            _dbConfig = config.GetSection("Database").Get<DatabaseConfig>();
        }



        static string Urlize(string name)
        {
            name = Transliteration.LatinToCyrillic(name, Language.Russian);

            StringBuilder sb = new StringBuilder();
            var arrayText = name.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (
                    (letter >= 'a' && letter <= 'z')
                    || (letter >= 'A' && letter <= 'Z')
                    || (letter >= '0' && letter <= '9')
                    || letter == '-'
                )
                    sb.Append(letter);
                if (letter == ' ')
                    sb.Append("-");
            }

            return sb.ToString();
        }


        protected override Task<int> Execute(ImportCmdletOptions args) =>
            _db.ExecAsync(async db =>
            {
                MigrationRunner.MigrateDb(_dbConfig.ConnectionString, typeof(Startup).Assembly, _dbConfig.Type);
                AppDbContextSeed.Seed(_db);

                
                var import = JsonConvert.DeserializeObject<ImporterDatabase>(
                    await File.ReadAllTextAsync(args.Input));
                
                var langs = db.Languages.ToDictionary(x => x.LanguageShortName, x => x.Id);

                await db.UniversityLanguages.TruncateAsync();
                await db.UniversityInstructionLanguages.TruncateAsync();
                await db.CommonTraitsToUniversities.TruncateAsync();
                await db.ExecuteAsync("TRUNCATE TABLE \"Universities\" CASCADE");

                await ImportUniversities(import, db, langs);
                await ImportCities(db, import);
                await ImportSpecialties(db, import);
                
                return 0;
            });


        private async Task<int> EnsureTraitType(AppDbContext db, string identifier, object names)
        {
            var traitType = db.CommonTraitTypes.FirstOrDefault(x => x.Identifier == identifier);
            if (traitType == null)
            {
                Console.WriteLine($"Adding {identifier} trait type");
                traitType = new CommonTraitType
                {
                    Identifier = "degree",
                    Names = JsonConvert.SerializeObject(names),
                };
                traitType.Id = await db.InsertWithInt32IdentityAsync(traitType);
                await db.InsertAsync(new CommonTraitTypesForEntity
                {
                    EntityType = EntityType.University,
                    CommonTraitId = traitType.Id
                });
            }

            return traitType.Id;
        }

        private  async Task ImportUniversities(ImporterDatabase import, AppDbContext db, Dictionary<string, int> langs)
        {
            var degreeTraitType = await EnsureTraitType(db, "degree", new
            {
                en = "Degree", ru = "Образование", esp = "Grado", fr = "Degré"
            });

            var degreesToTraits = new Dictionary<ImporterUniversityDegree, int>();
            foreach (var t in new[]
            {
                (degree: ImporterUniversityDegree.Bachelor, names: new
                {
                    en = "Bachelor", ru = "Бакалавр", esp = "Soltero", fr = "Célibataire"
                }),
                (degree: ImporterUniversityDegree.Magistracy, names: new
                {
                    en = "Magistracy", ru = "Магистратура", esp = "Magistratura", fr = "Magistrature"
                }),
                (degree: ImporterUniversityDegree.Mba, names: new
                {
                    en = "MBA", ru = "MBA", esp = "MBA", fr = "MBA"
                }),
                (degree: ImporterUniversityDegree.Summer, names: new
                {
                    en = "Summer courses", ru = "Летние / краткосрочные программы", esp = "Cursos de verano", fr = "Cours d'été"
                }),
                    
            })
            {
                var identifier = t.degree.ToString().ToLowerInvariant();
                var trait = db.CommonTraits.FirstOrDefault(x =>
                    x.CommonTraitTypeId == degreeTraitType && x.Identifier == identifier);
                if (trait != null)
                    degreesToTraits[t.degree] = trait.Id;
                else
                {
                    Console.WriteLine("Adding degree " + t.degree);
                    degreesToTraits[t.degree] = await db.InsertWithInt32IdentityAsync(new CommonTrait
                    {
                        Identifier = identifier,
                        Names = JsonConvert.SerializeObject(t.names),
                        CommonTraitTypeId = degreeTraitType
                    });
                }
            }

            
            foreach (var uni in import.Universities)
            {
                Console.WriteLine("Importing " + uni.Id);

                await db.Universities
                    .Value(x => x.Id, uni.Id)
                    .Value(x => x.Website, uni.Website)
                    .Value(x => x.FoundationYear, uni.FoundationYear)
                    .Value(x => x.MinimumAge, uni.MinumumAge)
                    .InsertAsync();
                
                foreach (var lang in uni.Languages)
                    await db.InsertAsync(new UniversityLanguage
                    {
                        Url = Urlize(lang.Value.Name),
                        Description = lang.Value.HtmlData,
                        Name = lang.Value.Name,
                        LanguageId = langs[lang.Key],
                        UniversityId = uni.Id
                    });

                foreach (var lang in uni.LanguagesOfInstruction)
                    await db.InsertAsync(new UniversityInstructionLanguage()
                    {
                        LanguageId = langs[lang],
                        UniversityId = uni.Id
                        
                    });

                foreach (var degree in uni.Degrees)
                    await db.InsertAsync(new CommonTraitsToUniversity
                    {
                        UniversityId = uni.Id,
                        CommonTraitId = degreesToTraits[degree]
                    });

            }

            await db.ExecuteAsync(
                $"SELECT setval('\"Universities_Id_seq\"', {import.Universities.Max(x => x.Id)})");
        }

        private bool FindDbEntity<TDb>(List<TDb> dbList, Dictionary<string, string> names, Func<TDb,Dictionary<string, string>> getNames,
            Func<TDb, int> getId, out int id)
        {
            foreach (var db in dbList)
            foreach (var lang in names)
                if (getNames(db).TryGetValue(lang.Key, out var dbName)
                    && dbName.Trim().ToLowerInvariant() == lang.Value.Trim().ToLowerInvariant())
                {
                    id = getId(db);
                    return true;
                }

            id = 0;
            return false;
        }
        
        private async Task ImportCities(AppDbContext db, ImporterDatabase import)
        {
            var cityTraitType = await EnsureTraitType(db, "city", new
            {
                en = "City", ru = "Город", esp = "Ciudad", fr = "Ville"
            });

            var citiesDic = new Dictionary<int, int>();
            var dbCities = db.CommonTraits.Where(ct => ct.CommonTraitTypeId == cityTraitType)
                .ToList()
                .Select(x => new
                    {Id = x.Id, Names = JsonConvert.DeserializeObject<Dictionary<string, string>>(x.Names)}).ToList();

            foreach (var city in import.Cities)
            {
                if (FindDbEntity(dbCities, city.Names, x => x.Names, x => x.Id, out var dbCityId))
                    citiesDic[city.Id] = dbCityId;
                else
                {
                    Console.WriteLine("Adding city " + string.Join(", ", city.Names.Values));
                    citiesDic[city.Id] = await db.InsertWithInt32IdentityAsync(new CommonTrait()
                    {
                        CommonTraitTypeId = cityTraitType,
                        Names = JsonConvert.SerializeObject(city.Names)
                    });
                }
            }

            foreach (var uni in import.Universities)
            {
                foreach (var city in uni.Cities)
                    await db.InsertAsync(new CommonTraitsToUniversity
                    {
                        UniversityId = uni.Id,
                        CommonTraitId = citiesDic[city]
                    });
            }
        }

        private async Task ImportSpecialties(AppDbContext db, ImporterDatabase import)
        {
            var dbCategories = db.SpecialtyCategories.ToList();
            var dbSpecialties = db.Specialties.ToList();
            
            var specDic = new Dictionary<int, int>();
            foreach (var category in import.Specialties)
            {
                int categoryId;
                if (FindDbEntity(dbCategories, category.Names, x => x.Names, x => x.Id, out var dbCatId))
                    categoryId = dbCatId;
                else
                {
                    Console.WriteLine("Adding category " + string.Join(", ", category.Names.Values));
                    categoryId = await db.InsertWithInt32IdentityAsync(new SpecialtyCategory()
                    {
                        Names = category.Names
                    });
                }

                foreach (var spec in category.Specialties)
                {
                    if (FindDbEntity(dbSpecialties, spec.Names, x => x.Names, x => x.Id, out var dbSpecId))
                        specDic[spec.Id] = dbSpecId;
                    else
                    {
                        Console.WriteLine("Adding spec " + string.Join(", ", spec.Names.Values));
                        specDic[spec.Id] = await db.InsertWithInt32IdentityAsync(new Specialty()
                        {
                            Names = spec.Names,
                            CategoryId = categoryId
                        });
                    }
                }
            }
        }
    }
}