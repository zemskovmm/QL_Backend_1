using QuartierLatin.Backend.Dto.CommonTraitDto;
using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.CourseCatalogDto.Course.ModuleDto
{
    public class CourseModuleTraitsDto
    {
        public Dictionary<string, List<CommonTraitLanguageDto>> NamedTraits { get; set; }
    }
}
