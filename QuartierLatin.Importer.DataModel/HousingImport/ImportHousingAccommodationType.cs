using System.Collections.Generic;

namespace QuartierLatin.Importer.DataModel.HousingImport
{
    public class ImportHousingAccommodationType
    {
        public Dictionary<string, string> Names { get; set; }
        public string Square { get; set; }
        public string Residents { get; set; }
        public int Price { get; set; }

        public List<ImporterCommonTraits> CommonTraits { get; set; }
    }
}
