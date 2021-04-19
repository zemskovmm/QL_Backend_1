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
    [Verb("export-excel", HelpText = "Generate XLSX file with existing database values.")]
    // ReSharper disable once UnusedType.Global
    public class ExportXlsCommand : ICommandLineCommand
    {
        private ImporterDatabase _db;
        private List<string> _langs;
        private Dictionary<string, int> _langIndices;
        private Dictionary<int, ImporterUniversity> _unis;
        private Dictionary<int, ImporterCity> _cities;
        private Dictionary<int, ImporterSpecialty> _specs;

        [Option('i', "input", Required = true)]
        public string InputFile { get; set; }

        [Option('o', "output", Required = true)]
        public string OutputFile { get; set; }


        public int Execute()
        {
            _db = JsonConvert.DeserializeObject<ImporterDatabase>(File.ReadAllText(InputFile));
            if (_db.Specialties.Count == 0)
            {
                DetectDegrees();
                TemplatePlaceholders.InitializeWithPlaceholders(_db);
            }

            _langs = _db.Universities.SelectMany(u => u.Languages.Keys).Distinct().OrderBy(x => x).ToList();
            _langIndices = _langs.Select((x, i) => (x, i)).ToDictionary(x => x.x, x => x.i);
            _unis = _db.Universities.ToDictionary(x => x.Id);
            _cities = _db.Cities.ToDictionary(x => x.Id);
            _specs = _db.Specialties.SelectMany(x => x.Specialties).ToDictionary(x => x.Id);

            var doc = new XLWorkbook();
            GenerateUniversities(doc.AddWorksheet(XlsConstants.Universities));
            GenerateUniversityDegrees(doc.AddWorksheet(XlsConstants.UniversityDegrees));
            GenerateCities(doc.AddWorksheet(XlsConstants.Cities));
            GenerateUniversityCities(doc.AddWorksheet(XlsConstants.UniversityCities));
            GenerateSpecialties(doc.AddWorksheet(XlsConstants.Specialties));
            GenerateUniversitySpecialties(doc.AddWorksheet(XlsConstants.UniversitySpecialties));

            foreach (var sheet in doc.Worksheets)
            {
                sheet.Columns().AdjustToContents();
                foreach (var col in sheet.ColumnsUsed())
                    col.Cell(1).Style.Font.SetBold();
            }
            
            doc.SaveAs(OutputFile);
            return 0;
        }

        void DetectDegrees()
        {
            foreach (var uni in _db.Universities)
            {
                bool ContainsAnyOf(params string[] s) => uni.Languages.Any(l =>
                {
                    var lower = l.Value.HtmlData.ToLowerInvariant();
                    return s.Any(x => lower.Contains(x));
                });
                
                if (ContainsAnyOf("mba"))
                    uni.Degrees.Add(ImporterUniversityDegree.Mba);
                if(ContainsAnyOf("bachelor", "бакалавр"))
                    uni.Degrees.Add(ImporterUniversityDegree.Bachelor);
                if(ContainsAnyOf("летняя", "летние"))
                    uni.Degrees.Add(ImporterUniversityDegree.Summer);
                if (ContainsAnyOf("магистрат", "magistracy", "magister"))
                    uni.Degrees.Add(ImporterUniversityDegree.Magistracy);
            }
        }
        
        void WriteLangHeaders(IXLWorksheet sheet, int startIndex)
        {
            foreach (var li in _langIndices)
                sheet.Cell(1, startIndex + li.Value).Value = li.Key;
        }

        void WriteLangs(IXLWorksheet sheet, int row, int startIndex, Func<string, string> cb)
        {
            for (var c = 0; c < _langs.Count; c++)
                sheet.Cell(row, startIndex + c).Value =
                    cb(_langs[c]);
        }
        
        void GenerateUniversities(IXLWorksheet sheet)
        {
            sheet.Cell(1, XlsConstants.UniversityIdColumn).Value = "Id";
            sheet.Cell(1, XlsConstants.UniversityYearColumn).Value = "Year";
            sheet.Cell(1, XlsConstants.UniversityWebsiteColumn).Value = "Website";
            sheet.Cell(1, XlsConstants.UniversityMinimalAgeColumn).Value = "MinAge";
            sheet.Cell(1, XlsConstants.UniversityLangsColumn).Value = "Langs";
            
            WriteLangHeaders(sheet, XlsConstants.UniversityFirstLangColumn);
            var row = 1;
            foreach (var uni in _db.Universities.OrderBy(x => x.Id))
            {
                row++;
                sheet.Cell(row, XlsConstants.UniversityIdColumn).Value = uni.Id;
                sheet.Cell(row, XlsConstants.UniversityYearColumn).Value = uni.FoundationYear;
                sheet.Cell(row, XlsConstants.UniversityWebsiteColumn).Value = uni.Website;
                sheet.Cell(row, XlsConstants.UniversityMinimalAgeColumn).Value = uni.MinumumAge;
                sheet.Cell(row, XlsConstants.UniversityLangsColumn).Value =
                    string.Join(",", uni.LanguagesOfInstruction);

                WriteLangs(sheet, row, XlsConstants.UniversityFirstLangColumn,
                    l => uni.Languages.GetValueOrDefault(l)?.Name);
            }
        }

        string GetBestName(Dictionary<string, string> dic)
        {
            foreach (var l in new[] {"fr", "ru", "en"})
                if (dic.TryGetValue(l, out var n))
                    return n;
            return dic.Values.First();
        }
        
        void WriteUniversityHeader(IXLCell cell, ImporterUniversity uni)
        {
            string name = uni.Languages.First().Value.Name;
            foreach (var l in new[] {"fr", "ru", "en"})
            {
                if (uni.Languages.TryGetValue(l, out var n))
                {
                    name = n.Name;
                    break;
                }
            }

            cell.Value = uni.Id + " " + name;
        }
        
        void GenerateUniversityDegrees(IXLWorksheet sheet)
        {
            sheet.Cell(1, 1).Value = "University";
            var degrees = Enum.GetValues<ImporterUniversityDegree>().ToList();
            for (var c = 0; c < degrees.Count; c++)
                sheet.Cell(1, 2 + c).Value = degrees[c].ToString();
            var row = 1;
            foreach (var uni in _db.Universities.OrderBy(x => x.Id))
            {
                row++;
                WriteUniversityHeader(sheet.Cell(row, 1), uni);
                for (var c = 0; c < degrees.Count; c++)
                    sheet.Cell(row, 2 + c).Value = uni.Degrees.Contains(degrees[c]) ? "x" : "";
            }
        }
        
        void GenerateCities(IXLWorksheet sheet)
        {
            WriteLangHeaders(sheet, 1);
            var row = 1;
            foreach (var city in _db.Cities)
            {
                row++;
                WriteLangs(sheet, row, 1, l => city.Names.GetValueOrDefault(l));
            }
        }
        
        void GenerateUniversityCities(IXLWorksheet sheet)
        {
            sheet.Cell(1, 1).Value = "University";
            var row = 1;
            foreach (var uni in _db.Universities.OrderBy(x => x.Id))
            {
                row++;
                WriteUniversityHeader(sheet.Cell(row, 1), uni);
                for (var c = 0; c < uni.Cities.Count; c++)
                {
                    var city = _cities[uni.Cities[c]];
                    sheet.Cell(row, c + 2).Value = GetBestName(city.Names);
                }
            }
        }


        void GenerateSpecialties(IXLWorksheet sheet)
        {
            sheet.Cell(1, 1).Value = "IsGroup?";
            foreach (var li in _langIndices)
                sheet.Cell(1, 2 + li.Value).Value = li.Key;
            
            var row = 1;
            foreach (var cat in _db.Specialties)
            {
                row++;
                sheet.Cell(row, 1).Value = "x";
                WriteLangs(sheet, row, 2, l => cat.Names.GetValueOrDefault(l));
                foreach (var spec in cat.Specialties)
                {
                    row++;
                    WriteLangs(sheet, row, 2, l => spec.Names.GetValueOrDefault(l));
                }
            }            
            foreach (var li in _langIndices)
                sheet.Cell(1, 2 + li.Value).Value = li.Key;
        }

        void GenerateUniversitySpecialties(IXLWorksheet sheet)
        {
            sheet.Cell(1, 1).Value = "University";
            sheet.Cell(1, 2).Value = "Specialty";
            sheet.Cell(1, 3).Value = "Cost";
            var row = 1;
            foreach (var uni in _db.Universities.OrderBy(x => x.Id))
            {
                foreach (var mapping in uni.Specialties)
                {
                    row++;
                    WriteUniversityHeader(sheet.Cell(row, 1), uni);
                    sheet.Cell(row, 2).Value = GetBestName(_specs[mapping.SpecialtyId].Names);
                    sheet.Cell(row, 3).Value = mapping.Cost;
                }
            }
        }
    }
}