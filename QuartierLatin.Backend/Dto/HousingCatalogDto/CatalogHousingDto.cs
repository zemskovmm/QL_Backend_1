using QuartierLatin.Backend.Dto.CommonTraitDto;
using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.HousingCatalogDto
{
    public class CatalogHousingDto
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string LanglessUrl { get; set; }
        public List<int> GalleryList { get; set; }
        public int? ImageId { get; set; }
        public int? Price { get; set; }
        public Dictionary<string, List<CommonTraitLanguageDto>>? NamedTraits { get; set; }
    }
}
