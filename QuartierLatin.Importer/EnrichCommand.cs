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
        private List<string> _langs;
        private Dictionary<string, int> _langIndices;
        private Dictionary<int, ImporterUniversity> _unis;
        private Dictionary<int, ImporterCity> _cities;
        private Dictionary<int, ImporterSpecialty> _specs;
        
        [Option('i', "input", Required = true)] public string InputFile { get; set; }
        [Option('e', "excel", Required = true)] public string ExcelFile { get; set; }
        [Option('o', "output", Required = true)] public string OutputFile { get; set; }
        public int Execute()
        {
            _db = JsonConvert.DeserializeObject<ImporterDatabase>(File.ReadAllText(InputFile));
            _langs = _db.Universities.SelectMany(u => u.Languages.Keys).Distinct().OrderBy(x => x).ToList();
            _langIndices = _langs.Select((x, i) => (x, i)).ToDictionary(x => x.x, x => x.i);
            
            
            var doc = new XLWorkbook(ExcelFile);
            IXLWorksheet FindSheet(string name) => doc.Worksheets.First(x => x.Name == name);
            
            _unis = _db.Universities.ToDictionary(x => x.Id);
            LoadUniversities(FindSheet(XlsConstants.Universities));
            LoadUniversityDegrees(FindSheet(XlsConstants.UniversityDegrees));
            LoadCities(FindSheet(XlsConstants.Cities));
            _cities = _db.Cities.ToDictionary(x => x.Id);
            LoadUniversityCities(FindSheet(XlsConstants.UniversityCities));
            
            LoadSpecialties(FindSheet(XlsConstants.Specialties));
            _specs = _db.Specialties.SelectMany(x => x.Specialties).ToDictionary(x => x.Id);
            LoadUniversitySpecialties(FindSheet(XlsConstants.UniversitySpecialties));
            
            File.WriteAllText(OutputFile, JsonConvert.SerializeObject(_db, Formatting.Indented));
            
            return 0;
        }

        int FindHeader(IXLWorksheet sheet, string name) => sheet.ColumnsUsed()
            .First(c => c.Cell(1).Value?.ToString() == name).RangeAddress.FirstAddress.ColumnNumber;

        
        void LoadUniversities(IXLWorksheet sheet)
        {
            var iId = FindHeader(sheet, "Id");
            var iYear = FindHeader(sheet, "Year");
            var iWebsite = FindHeader(sheet, "Website");
            var iMinAge = FindHeader(sheet, "MinAge");
            var iLangs = FindHeader(sheet, "Langs");
            var row = 2;
            while(sheet.Row(row).CellsUsed().Any())
            {
                var id = sheet.GetInt(row, iId);
                
                var uni = _unis[id];
                uni.FoundationYear = sheet.GetNInt(row, iYear);
                uni.Website =sheet.GetString(row, iWebsite);
                uni.MinumumAge = sheet.GetNInt(row, iMinAge);
                uni.LanguagesOfInstruction = (sheet.GetString(row, iLangs) ?? "")
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
                row++;
            }
        }
        
        void LoadUniversityDegrees(IXLWorksheet sheet)
        {
            var indices = new Dictionary<int, ImporterUniversityDegree>();
            for (var c = 2;; c++)
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
                _unis[id].Degrees = indices.Where(i => sheet.Cell(row, i.Key).Value?.ToString().Trim().Length > 0)
                    .Select(i => i.Value).ToList();
                row++;
            }
        }
        
        void LoadCities(IXLWorksheet sheet)
        {
            _db.Cities = new List<ImporterCity>();
            var langs = ReadLangHeaders(sheet, 1);
            var row = 2;
            var nextCityId = 1;
            while (sheet.Row(row).CellsUsed().Any())
            {
                _db.Cities.Add(new ImporterCity
                {
                    Id = nextCityId++,
                    Names = ReadLangs(sheet, langs, row)
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
        
        void LoadUniversityCities(IXLWorksheet sheet)
        {
            var row = 2;
            while (sheet.Row(row).CellsUsed().Any())
            {
                var id = int.Parse(sheet.Cell(row, 1).Value.ToString().Split(' ')[0]);
                var uni = _unis[id];
                uni.Cities.Clear();
                for (var c = 2;; c++)
                {
                    var v = sheet.GetString(row, c);
                    if (string.IsNullOrWhiteSpace(v))
                        break;
                    var city = FindWithName(_db.Cities, v, x => x.Names);
                    if (city == null)
                        throw new ImporterException($"City {v} not found", sheet, row, c);
                    uni.Cities.Add(city.Id);
                }
                row++;
            }
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

        void LoadUniversitySpecialties(IXLWorksheet sheet)
        {
            var row = 2;
            foreach(var uni in _db.Universities)
                uni.Specialties.Clear();
            while (sheet.Row(row).CellsUsed().Any())
            {
                var id = int.Parse(sheet.GetString(row, 1).Split(' ')[0]);
                var uni = _unis[id];
                
                var specName = sheet.GetString(row, 2);
                var spec = FindWithName(_specs.Values, specName, x => x.Names);
                if (spec == null)
                    throw new ImporterException("Spec not found " + specName, sheet, row, 2);
                var cost = sheet.GetInt(row, 3);
                uni.Specialties.Add(new ImporterUniversitySpecialtyMapping
                {
                    SpecialtyId = spec.Id,
                    Cost = cost
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