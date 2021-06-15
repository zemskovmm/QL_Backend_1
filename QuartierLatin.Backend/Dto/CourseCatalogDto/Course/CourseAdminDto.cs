using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.CourseCatalogDto.Course
{
    public class CourseAdminDto
    {
        public int SchoolId { get; set; }
        public Dictionary<string, CourseLanguageAdminDto> Languages { get; set; }
    }
}
