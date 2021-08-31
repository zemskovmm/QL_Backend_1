using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto.RouteDto;
using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.HousingCatalogDto.RouteDto
{
    public class HousingModuleDto
    {
        public int? Price { get; set; }
        public string Title { get; set; }
        public string HtmlDescription { get; set; }
        public int? ImageId { get; set; }
        public NamedTraitsModuleDto Traits { get; set; }
        public IEnumerable<int> GalleryList { get; set; }
        public JObject? Metadata { get; set; }
        public IEnumerable<HousingAccommodationTypeModuleDto> HousingAccommodationTypes { get; set; }
    }
}
