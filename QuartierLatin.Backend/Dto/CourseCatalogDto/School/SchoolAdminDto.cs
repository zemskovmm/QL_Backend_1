using System.Collections.Generic;
using RemoteUi;

namespace QuartierLatin.Backend.Dto.CourseCatalogDto.School
{
    public class SchoolAdminDto : BaseDto
    {
        [RemoteUiField("foundationYear")]
        public int? FoundationYear { get; set; }
        [RemoteUiField("languages", "", RemoteUiFieldType.Custom, CustomType = "LanguageDictionary")]
        public Dictionary<string, SchoolLanguageAdminDto> Languages { get; set; }
        [RemoteUiField("imageId")]
        public int? ImageId { get; set; }
    }
}
