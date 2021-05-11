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
using QuartierLatin.Backend.Utils;
using QuartierLatin.Importer;
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

        protected override Task<int> Execute(ImportCmdletOptions args) =>
            _db.ExecAsync(async db =>
            {
                MigrationRunner.MigrateDb(_dbConfig.ConnectionString, typeof(Startup).Assembly, _dbConfig.Type);
                AppDbContextSeed.Seed(_db);


                var import = JsonConvert.DeserializeObject<ImporterDatabase>(
                    (await File.ReadAllTextAsync(args.Input))
                    // Some people aren't aware of ISO codes
                    .Replace("\"ch\"", "\"cn\""));
                
                var langs = db.Languages.ToDictionary(x => x.LanguageShortName, x => x.Id);

                await db.UniversityLanguages.TruncateAsync();
                await db.CommonTraitsToUniversities.TruncateAsync();
                await db.ExecuteAsync("TRUNCATE TABLE \"Universities\" CASCADE");

                await ImportUniversities(import, db, langs);
                await ImportDegrees(db, import);
                await ImportNamedTraits(db, "city", import.Cities, import.Universities, u => u.Cities,
                    new Dictionary<string, string>
                    {
                        {"en", "City"}, {"ru", "Город"}, {"esp", "Ciudad"}, {"fr", "Ville"}
                    });
                await ImportNamedTraits(db, "accreditation", import.Accreditations, import.Universities, u => u.Accreditations,
                    new Dictionary<string, string>
                    {
                        {"en", "Accreditation"}, {"ru", "Аккредитация"}, {"esp", "Acreditación"}, {"fr", "Accréditation"}
                    });
                await ImportNamedTraits(db, "certification", import.Certifications, import.Universities, u => u.Certifications,
                    new Dictionary<string, string>
                    {
                        {"en", "Certification"}, {"ru", "Сертификация"}, {"esp", "Certificación"}, {"fr", "Certification"}
                    });
                await ImportSpecialties(db, import);
                
                return 0;
            });


        private async Task<int> EnsureTraitType(AppDbContext db, string identifier, Dictionary<string, string> names)
        {
            var traitType = db.CommonTraitTypes.FirstOrDefault(x => x.Identifier == identifier);
            if (traitType == null)
            {
                Console.WriteLine($"Adding {identifier} trait type");
                traitType = new CommonTraitType
                {
                    Identifier = identifier,
                    Names = names,
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

        private async Task<int> EnsureTraitWithIdentifier(AppDbContext db, int traitTypeId, string identifier,
            Dictionary<string, string> names)
        {
            var trait = db.CommonTraits.FirstOrDefault(x =>
                x.CommonTraitTypeId == traitTypeId && x.Identifier == identifier);
            if (trait != null)
                return trait.Id;
            
            Console.WriteLine("Adding " + identifier);
            return await db.InsertWithInt32IdentityAsync(new CommonTrait
            {
                Identifier = identifier,
                Names = names,
                CommonTraitTypeId = traitTypeId
            });
        }

        private  async Task ImportUniversities(ImporterDatabase import, AppDbContext db, Dictionary<string, int> langs)
        {
            var languagesToTraits = new Dictionary<string, int>();
            var languageTraitId = await EnsureTraitType(db, "instruction-language", new Dictionary<string, string>
            {
                {"en", "Language"}, {"ru", "Язык обучения"}, {"esp", "Idioma"}, {"fr", "Langue"}
            });
            foreach (var lang in new[]
            {
                (lang: "ru", names: new Dictionary<string, string>
                    {{"ru", "Русский"}, {"en", "Russian"}, {"esp", "Ruso"}, {"fr", "Russe"}}),
                (lang: "en", names: new Dictionary<string, string>
                    {{"ru", "Английский"}, {"en", "English"}, {"esp", "Inglés"}, {"fr", "Anglais"}}),
                (lang: "esp", names: new Dictionary<string, string>
                    {{"ru", "Испанский"}, {"en", "Spanish"}, {"esp", "Español"}, {"fr", "Espanol"}}),
                (lang: "fr", names: new Dictionary<string, string>
                    {{"ru", "Французский"}, {"en", "French"}, {"esp", "Francés"}, {"fr", "Français"}}),
                (lang: "fr/en", names: new Dictionary<string, string>
                    {{"ru", "Французский/Английский"}, {"en", "French/English"}, {"esp", "Francés/Inglés"}, {"fr", "Français/Anglais"}}),
            }) 
                languagesToTraits[lang.lang] = await EnsureTraitWithIdentifier(db, languageTraitId, lang.lang, lang.names);


            foreach (var uni in import.Universities)
            {
                Console.WriteLine("Importing " + uni.Id);

                await db.Universities
                    .Value(x => x.Id, uni.Id)
                    .Value(x => x.FoundationYear, uni.FoundationYear)
                    .InsertAsync();
                
                foreach (var lang in uni.Languages)
                    await db.InsertAsync(new UniversityLanguage
                    {
                        Url = Urlizer.Urlize(lang.Value.Name),
                        Description = lang.Value.HtmlData,
                        Name = lang.Value.Name,
                        LanguageId = langs[lang.Key],
                        UniversityId = uni.Id
                    });

                foreach (var lang in uni.LanguagesOfInstruction)
                {
                    await db.InsertAsync(new CommonTraitsToUniversity
                    {
                        UniversityId = uni.Id,
                        CommonTraitId = languagesToTraits[lang]
                    });
                }
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
        
        private async Task ImportNamedTraits<TImporter>(AppDbContext db, string identifier, 
            List<TImporter> list,
            List<ImporterUniversity> universities,
            Func<ImporterUniversity, List<int>> idList,
            Dictionary<string, string> traitNames)
            where TImporter : ImporterNamedEntityBase
        {
            var traitType = await EnsureTraitType(db, identifier, traitNames);

            var itemsDic = new Dictionary<int, int>();
            var dbTraits = db.CommonTraits.Where(ct => ct.CommonTraitTypeId == traitType)
                .ToList()
                .Select(x => new
                    {Id = x.Id, Names = x.Names}).ToList();

            foreach (var item in list)
            {
                if (FindDbEntity(dbTraits, item.Names, x => x.Names, x => x.Id, out var dbCityId))
                    itemsDic[item.Id] = dbCityId;
                else
                {
                    Console.WriteLine($"Adding {identifier} " + string.Join(", ", item.Names.Values));
                    itemsDic[item.Id] = await db.InsertWithInt32IdentityAsync(new CommonTrait()
                    {
                        CommonTraitTypeId = traitType,
                        Names = item.Names
                    });
                }
            }

            foreach (var uni in universities)
            {
                foreach (var city in idList(uni))
                    await db.InsertAsync(new CommonTraitsToUniversity
                    {
                        UniversityId = uni.Id,
                        CommonTraitId = itemsDic[city]
                    });
            }
        }

        private async Task ImportDegrees(AppDbContext db, ImporterDatabase import)
        {
            await db.UniversityDegrees.TruncateAsync();
            await db.Degrees.DeleteAsync();
            
            await db.ExecuteAsync("ALTER SEQUENCE \"Degrees_Id_seq\" RESTART WITH 1;");
            //await db.ExecuteAsync("SELECT setval('\"Degrees_Id_seq\"', 1)");
            var degreesDic = new Dictionary<ImporterUniversityDegree, int>();
            foreach (var t in new[]
            {
                (degree: ImporterUniversityDegree.Bachelor, names: new Dictionary<string, string>
                {
                    {"en", "Bachelor"}, {"ru", "Бакалавр"}, {"esp", "Soltero"}, {"fr", "Célibataire"}
                }),
                (degree: ImporterUniversityDegree.Magistracy, names: new Dictionary<string, string>
                {
                    {"en", "Magistracy"}, {"ru", "Магистратура"}, {"esp", "Magistratura"}, {"fr", "Magistrature"}
                }),
                (degree: ImporterUniversityDegree.Mba, names: new Dictionary<string, string>
                {
                    {"en", "MBA"}, {"ru", "MBA"}, {"esp", "MBA"}, {"fr", "MBA"}
                }),
                (degree: ImporterUniversityDegree.Short, names: new Dictionary<string, string>
                {
                    {"en", "Summer/short courses"}, {"ru", "Летние / краткосрочные программы"}, {"esp", "Cursos de verano"}, {"fr", "Cours d'été"}
                }),
                    
            })
            {
                var id = await db.InsertWithInt32IdentityAsync(new Degree
                {
                    Names = t.names
                });
                degreesDic[t.degree] = id;
            }
            foreach(var uni in import.Universities)
            foreach (var map in uni.Degrees)
                await db.InsertAsync(new UniversityDegree
                {
                    UniversityId = uni.Id,
                    DegreeId = degreesDic[map.Key],
                    CostGroup = map.Value
                });
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

            db.UniversitySpecialties.Truncate();
            foreach (var uni in import.Universities)
            {
                foreach (var spec in uni.Specialties)
                    db.Insert(new UniversitySpecialty
                    {
                        SpecialtyId = specDic[spec],
                        UniversityId = uni.Id
                    });
            }
        }
    }
}