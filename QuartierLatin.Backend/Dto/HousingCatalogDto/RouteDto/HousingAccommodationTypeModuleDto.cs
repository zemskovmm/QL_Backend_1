using QuartierLatin.Backend.Dto.RouteDto;

namespace QuartierLatin.Backend.Dto.HousingCatalogDto.RouteDto
{
    public class HousingAccommodationTypeModuleDto
    {
        public string Name { get; set; }
        public string Square { get; set; }
        public string Residents { get; set; }
        public int Price { get; set; }
        public NamedTraitsModuleDto Traits { get; set; }
    }
}
