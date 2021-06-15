using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.courseCatalogDto.School
{
    public class SchoolListAdminDto : BaseDto
    {
        public int? FoundationYear { get; set; }
        public Dictionary<string, SchoolLanguageAdminDto> Languages { get; set; }
    }
}
