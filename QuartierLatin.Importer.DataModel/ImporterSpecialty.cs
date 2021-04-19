using System.Collections.Generic;

namespace QuartierLatin.Importer.DataModel
{
    public class ImporterSpecialty
    {
        public int Id { get; set; }
        public Dictionary<string, string> Names { get; set; } = new();
    }
}