using QuartierLatin.Backend.Dto.CourseCatalogDto.Course.ModuleDto;
using QuartierLatin.Backend.Dto.CourseCatalogDto.School.ModuleDto;

namespace QuartierLatin.Backend.Dto.CourseCatalogDto.RouteDto
{
    public class SchoolCourseModuleDto
    {
        public SchoolModuleDto School { get; set; }
        public CourseModuleDto Course { get; set; }
    }
}
