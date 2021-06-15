using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.CourseCatalogDto.Course
{
    public class CourseListAdminDto : BaseDto
    {
        public int SchoolId { get; set; }
        public Dictionary<string, CourseLanguageAdminDto> Languages { get; set; }
    }
}
