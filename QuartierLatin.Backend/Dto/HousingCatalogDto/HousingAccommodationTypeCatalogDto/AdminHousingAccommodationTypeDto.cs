using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.HousingCatalogDto.HousingAccommodationTypeCatalogDto
{
    public class AdminHousingAccommodationTypeDto : BaseDto
    {
        public Dictionary<string, string> Names { get; set; }
        public string Square { get; set; }
        public string Residents { get; set; }
        public int Price { get; set; }
        public int HousingId { get; set; }
    }
}
