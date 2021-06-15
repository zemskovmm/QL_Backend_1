using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.CourseCatalogDto.School
{
    public class SchoolAdminDto
    {
        public int? FoundationYear { get; set; }
        public Dictionary<string, SchoolLanguageAdminDto> Languages { get; set; }
    }
}
