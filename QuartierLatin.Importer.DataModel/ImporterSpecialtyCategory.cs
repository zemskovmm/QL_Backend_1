using System.Collections.Generic;

namespace QuartierLatin.Importer.DataModel
{
    public class ImporterSpecialtyCategory
    {
        public int Id { get; set; }
        public Dictionary<string, string> Names { get; set; } = new();
        public List<ImporterSpecialty> Specialties { get; set; } = new();
    }
}