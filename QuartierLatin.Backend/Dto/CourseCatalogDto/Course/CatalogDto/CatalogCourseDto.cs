using System.Collections.Generic;
using QuartierLatin.Backend.Dto.CommonTraitDto;

namespace QuartierLatin.Backend.Dto.CourseCatalogDto.Course.CatalogDto
{
    public class CatalogCourseDto
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string LanglessUrl { get; set; }
        public int? CourseImageId { get; set; }
        public int? SchoolImageId { get; set; }
        public string SchoolName { get; set; }
        public Dictionary<string, List<CommonTraitLanguageDto>>? NamedTraits { get; set; }
    }
}
