using System.Collections.Generic;
using RemoteUi;

namespace QuartierLatin.Backend.Dto.CourseCatalogDto.School
{
    public class SchoolAdminDto 
    {
        [RemoteUiField("id")]
        public int Id { get; set; }
        [RemoteUiField("foundationYear")]
        public int? FoundationYear { get; set; }
        [RemoteUiField("languages", "", RemoteUiFieldType.Custom, CustomType = "LanguageDictionary")]
        public Dictionary<string, SchoolLanguageAdminDto> Languages { get; set; }
    }
}
