using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.HousingCatalogDto
{
    public class HousingListAdminDto : BaseDto
    {
        public int? Price { get; set; }
        public Dictionary<string, HousingLanguageAdminDto> Languages { get; set; }
    }
}
