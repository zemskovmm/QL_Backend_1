using System.Collections.Generic;
using RemoteUi;

namespace QuartierLatin.Backend.Dto.HousingCatalogDto.HousingAccommodationTypeCatalogDto
{
    public class AdminHousingAccommodationTypeDto : BaseDto
    {
        [RemoteUiField("names", "", RemoteUiFieldType.Custom, CustomType = "PlainDictionary")]
        public Dictionary<string, string> Names { get; set; }
        [RemoteUiField("square")]
        public string Square { get; set; }
        [RemoteUiField("residents")]
        public string Residents { get; set; }
        [RemoteUiField("price")]
        public int Price { get; set; }
        [RemoteUiField("housingId")]
        public int HousingId { get; set; }
    }
}
