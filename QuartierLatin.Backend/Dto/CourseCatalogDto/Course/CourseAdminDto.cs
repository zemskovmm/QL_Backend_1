using System.Collections.Generic;
using RemoteUi;

namespace QuartierLatin.Backend.Dto.CourseCatalogDto.Course
{
    public class CourseAdminDto
    {
        public int Id { get; set; }
        [RemoteUiField("School Id")]
        public int SchoolId { get; set; }
        
        [RemoteUiField("languages", "", RemoteUiFieldType.Custom, CustomType = "LanguageDictionary")]
        public Dictionary<string, CourseLanguageAdminDto> Languages { get; set; }
    }
}
