using System.Collections.Generic;

namespace QuartierLatin.Importer.DataModel.HousingImport
{
    public class ImporterHousing
    {
        public int Price { get; set; }
        public Dictionary<string, ImporterHousingLanguage> HousingLanguage { get; set; }
        public List<ImportHousingAccommodationType> HousingAccommodation { get; set; }
        public List<ImporterCommonTraits> CommonTraits { get; set; }
        public List<string> FileNames { get; set; }
    }
}
