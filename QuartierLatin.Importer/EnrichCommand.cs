using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using CommandLine;
using Newtonsoft.Json;
using QuartierLatin.Importer.DataModel;

namespace QuartierLatin.Importer
{
    [Verb("enrich", HelpText = "Enrich university database with XLSX values.")]
    // ReSharper disable once UnusedType.Global
    class EnrichCommand: ICommandLineCommand
    {
        private ImporterDatabase _db;
        private List<string> _langs = new() {"en", "fr", "ru", "esp", "cn"};
        private Dictionary<int, ImporterUniversity> _unis;
        private Dictionary<int, ImporterSpecialty> _specs;
        
        [Option('i', "input", Required = true)] public string InputFile { get; set; }
        [Option('e', "excel", Required = true)] public string ExcelFile { get; set; }
        [Option('o', "output", Required = true)] public string OutputFile { get; set; }
        public int Execute()
        {
            _db = JsonConvert.DeserializeObject<ImporterDatabase>(File.ReadAllText(InputFile));
            
            var doc = new XLWorkbook(ExcelFile);
            IXLWorksheet FindSheet(string name) => doc.Worksheets.First(x => x.Name == name);
            
            _unis = _db.Universities.ToDictionary(x => x.Id);
            LoadNamedEntities(FindSheet(XlsConstants.Countries), _db.Countries);
            LoadUniversities(FindSheet(XlsConstants.Universities));
            
            LoadUniversityDegrees(FindSheet(XlsConstants.UniversityDegrees));

            LoadUniversityNamedEntities(FindSheet(XlsConstants.Cities), _db.Cities,
                FindSheet(XlsConstants.UniversityCities), u => u.Cities);


            LoadUniversityNamedEntities(FindSheet(XlsConstants.Certifications), _db.Certifications,
                FindSheet(XlsConstants.UniversityCertifications), uni => uni.Certifications, true);
            LoadUniversityNamedEntities(FindSheet(XlsConstants.Accreditations), _db.Accreditations,
                FindSheet(XlsConstants.UniversityAccreditations), uni => uni.Accreditations, true);
            
            LoadSpecialties(FindSheet(XlsConstants.Specialties));
            _specs = _db.Specialties.SelectMany(x => x.Specialties).ToDictionary(x => x.Id);
            LoadNamedEntitiesForUniversity(FindSheet(XlsConstants.UniversitySpecialties),
                _db.Specialties.SelectMany(x => x.Specialties).ToList(),
                uni => uni.Specialties, true);
            
            File.WriteAllText(OutputFile, JsonConvert.SerializeObject(_db, Formatting.Indented));
            
            return 0;
        }

        int FindHeader(IXLWorksheet sheet, string name) => sheet.ColumnsUsed()
            .First(c => c.Cell(1).Value?.ToString() == name).RangeAddress.FirstAddress.ColumnNumber;

        
        void LoadUniversities(IXLWorksheet sheet)
        {
            var iId = FindHeader(sheet, "Id");
            var iYear = FindHeader(sheet, "Year");
            var iLangs = FindHeader(sheet, "Langs");
            var iUrl = FindHeader(sheet, "Url");
            var iCountries = FindHeader(sheet, "Country");
            var row = 2;

            var langs = _langs.ToDictionary(l => l,
                l => sheet.Row(1).CellsUsed().First(c => c.Value.ToString() == l).Address.ColumnNumber);
            
            while(sheet.Row(row).CellsUsed().Any())
            {
                var id = sheet.GetInt(row, iId);
                if (!_unis.TryGetValue(id, out var uni))
                {
                    Console.WriteLine($"Warning: university #{id} not found, skipping");
                    row++;
                    continue;
                }

                foreach (var l in langs)
                {
                    var title = sheet.Cell(row, l.Value).Value?.ToString();
                    if (uni.Languages.TryGetValue(l.Key, out var version)) 
                        version.Name = title.IfSpace(version.Name);
                }
                
                uni.Url = sheet.GetString(row, iUrl);
                if (string.IsNullOrWhiteSpace(uni.Url))
                    uni.Url = Urlizer.Urlize(uni.Languages.OrderBy(x => _langs.IndexOf(x.Key)).First().Value.Name);
                
                uni.FoundationYear = sheet.GetNInt(row, iYear);
                
                uni.LanguagesOfInstruction = (sheet.GetString(row, iLangs) ?? "")
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

                foreach (var c in sheet.GetString(row, iCountries).Split(',',
                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                    uni.Countries.Add(GetWithName(_db.Countries, c).Id);

                row++;
            }
        }
        
        void LoadUniversityDegrees(IXLWorksheet sheet)
        {
            var indices = new Dictionary<int, ImporterUniversityDegree>();
            for (var c = 3;; c++)
            {
                var v = sheet.Cell(1, c).Value?.ToString();
                if(string.IsNullOrWhiteSpace(v))
                    break;
                indices[c] = Enum.Parse<ImporterUniversityDegree>(v);
            }
            
            var row = 2;
            while (sheet.Row(row).CellsUsed().Any())
            {
                var id = int.Parse(sheet.Cell(row, 1).Value.ToString().Split(' ')[0]);
                if (!_unis.TryGetValue(id, out var uni))
                {
                    Console.WriteLine($"Warning: university #{id} not foound, skipping");
                    row++;
                    continue;
                }
                uni.Degrees.Clear();
                foreach (var idx in indices)
                {
                    var v = sheet.Cell(row, idx.Key).Value?.ToString().Trim();
                    if (!string.IsNullOrWhiteSpace(v))
                    {
                        uni.Degrees[idx.Value] = v switch
                        {
                            "0-10" => 1,
                            "11-20" => 2,
                            "21-30" => 3,
                            "31-40" => 4,
                            "41-50" => 5,
                            "51-60" => 6,
                            _ => throw new ImporterException("Unknown price group " + v)
                        };
                    }
                }
                
                row++;
            }
        }

        void LoadNamedEntities<T>(IXLWorksheet sheet, List<T> to, bool noLang = false) where T : ImporterNamedEntityBase, new()
        {
            var langs = ReadLangHeaders(sheet, 1);
            var row = 2;
            var nextEntityId = 1;
            to.Clear();
            while (sheet.Row(row).CellsUsed().Any())
            {
                to.Add(new T
                {
                    Id = nextEntityId++,
                    Names = noLang
                        ? _langs.ToDictionary(x => x, _ => sheet.Cell(row, 1).Value.ToString())
                        : ReadLangs(sheet, langs, row)
                });
                row++;
            }
        }

        T FindWithName<T>(IEnumerable<T> list, string name, Func<T, Dictionary<string, string>> getNames)
        {
            var langs = new[] {"fr", "en", "ru"};
            langs = langs.Concat(_langs.Where(x => !langs.Contains(x))).ToArray();
            foreach (var l in langs)
            {
                foreach (var item in list)
                {
                    var names = getNames(item);
                    if (names.TryGetValue(l, out var itemName) &&
                        itemName.ToLowerInvariant().Trim() == name.ToLowerInvariant().Trim())
                        return item;
                }
            }
            return default;
        }

        T GetWithName<T>(IList<T> list, string name, bool throwIfNotFound = false) where T:ImporterNamedEntityBase, new()
        {
            var entity = FindWithName(list, name, x => x.Names);
            if (entity == null)
            {
                var err = $"entity {name} not found";
                if (throwIfNotFound)
                    throw new ImporterException(err);

                Console.WriteLine("Warning: " + err);
                entity = new T
                {
                    Id = list.Max(x => x.Id) + 1,
                    Names = _langs.ToDictionary(x => x, _ => name)
                };
                list.Add(entity);
            }

            return entity;
        }

        void LoadNamedEntitiesForUniversity<T>(IXLWorksheet sheet, List<T> entityList, Func<ImporterUniversity, List<int>> getList,
            bool throwIfNotFound = false)
            where T : ImporterNamedEntityBase, new()
        {
            var row = 2;
            while (sheet.Row(row).CellsUsed().Any())
            {
                var id = int.Parse(sheet.Cell(row, 1).Value.ToString().Split(' ')[0]);
                if (!_unis.TryGetValue(id, out var uni))
                {
                    Console.WriteLine($"Warning: university #{id} not found, skipping");
                    row++;
                    continue;
                }
                var list = getList(uni);
                list.Clear();
                for (var c = 3;; c++)
                {
                    var v = sheet.GetString(row, c);
                    if (string.IsNullOrWhiteSpace(v))
                        break;
                    var entity = GetWithName(entityList, v, throwIfNotFound);
                    list.Add(entity.Id);
                }
                row++;
            }
        }

        void LoadUniversityNamedEntities<T>(IXLWorksheet sourceSheet, List<T> to,
            IXLWorksheet mappingSheet,
            Func<ImporterUniversity, List<int>> getList,
            bool noLang = false) where T : ImporterNamedEntityBase, new()
        {
            LoadNamedEntities(sourceSheet, to, noLang);
            LoadNamedEntitiesForUniversity(mappingSheet, to, getList);
        }

        
        void LoadSpecialties(IXLWorksheet sheet)
        {
            var langs = ReadLangHeaders(sheet, 2);
            _db.Specialties.Clear();
            var nextCatId = 1;
            var nextSpecId = 1;
            var row = 2;
            ImporterSpecialtyCategory spec = null;
            
            while (sheet.Row(row).CellsUsed().Any())
            {
                var group = sheet.GetString(row, 1)?.ToLowerInvariant() == "x";
                if (group)
                    _db.Specialties.Add(spec = new ImporterSpecialtyCategory
                    {
                        Id = nextCatId++,
                        Names = ReadLangs(sheet, langs, row)
                    });
                else if (spec == null)
                    throw new ImporterException("Expected to have a category first", sheet, row, 1);
                else
                    spec.Specialties.Add(new ImporterSpecialty
                    {
                        Id = nextSpecId++,
                        Names = ReadLangs(sheet, langs, row)
                    });

                row++;
            }
        }
        
        Dictionary<int, string> ReadLangHeaders(IXLWorksheet sheet, int startIndex)
        {
            var dic = new Dictionary<int, string>();
            for (var c = startIndex;; c++)
            {
                var v = sheet.Cell(1, c).Value?.ToString();
                if(string.IsNullOrWhiteSpace(v))
                    break;
                dic[c] = v;
            }

            return dic;
        }

        Dictionary<string, string> ReadLangs(IXLWorksheet sheet, Dictionary<int, string> langs, int row) =>
            langs.Select(l => (l: l.Value, v: sheet.GetString(row, l.Key)))
                .Where(x => !string.IsNullOrWhiteSpace(x.v)).ToDictionary(x => x.l, x => x.v);

    }
}