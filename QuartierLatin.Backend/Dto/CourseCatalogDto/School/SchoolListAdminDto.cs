using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.CourseCatalogDto.School
{
    public class SchoolListAdminDto : BaseDto
    {
        public int? FoundationYear { get; set; }
        public int? ImageId { get; set; }
        public Dictionary<string, SchoolLanguageAdminDto> Languages { get; set; }
    }
}
