using System.Collections.Generic;

namespace QuartierLatin.Importer.DataModel.HousingImport
{
    public class ImporterCommonTraits
    {
        public Dictionary<string, string> Names { get; set; }
        public string CommonTraitTypeName { get; set; }
        public string IconBlobFileName { get; set; }
        public int Order { get; set; }
        public string Identifier { get; set; }
    }
}
