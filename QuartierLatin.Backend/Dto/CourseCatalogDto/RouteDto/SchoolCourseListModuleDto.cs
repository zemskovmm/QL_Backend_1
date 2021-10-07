using QuartierLatin.Backend.Dto.CourseCatalogDto.Course.ModuleDto;
using QuartierLatin.Backend.Dto.CourseCatalogDto.School.ModuleDto;
using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.CourseCatalogDto.RouteDto
{
    public class SchoolCourseListModuleDto
    {
        public SchoolModuleDto School { get; set; }
        public List<CourseListModuleDto> Courses { get; set; }
    }
}
