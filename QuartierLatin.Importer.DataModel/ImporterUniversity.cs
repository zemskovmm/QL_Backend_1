using System.Collections.Generic;

namespace QuartierLatin.Importer.DataModel
{
    public class ImporterUniversity
    {
        public int Id { get; set; }
        public Dictionary<string, ImporterUniversityLanguage> Languages { get; set; } = new();
        public int? FoundationYear { get; set; }
        public int? MinumumAge { get; set; }
        public string Website { get; set; }
        public List<string> LanguagesOfInstruction { get; set; } = new();
        public List<ImporterUniversityDegree> Degrees { get; set; } = new();
        public List<int> Cities { get; set; } = new();
        public List<ImporterUniversitySpecialtyMapping> Specialties { get; set; } = new();
    }

    public class ImporterCity
    {
        public int Id { get; set; }
        public Dictionary<string, string> Names { get; set; } = new();
    }
}