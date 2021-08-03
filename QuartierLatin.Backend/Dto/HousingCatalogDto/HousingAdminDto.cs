using RemoteUi;
using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.HousingCatalogDto
{
    public class HousingAdminDto : BaseDto
    {
        [RemoteUiField("price")]
        public int? Price { get; set; }
        [RemoteUiField("languages", "", RemoteUiFieldType.Custom, CustomType = "LanguageDictionary")]
        public Dictionary<string, HousingLanguageAdminDto> Languages { get; set; }
    }
}
