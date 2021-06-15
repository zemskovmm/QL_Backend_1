using QuartierLatin.Backend.Dto.CommonTraitDto;
using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.CourseCatalogDto.School.ModuleDto
{
    public class SchoolModuleTraitsDto
    {
        public Dictionary<string, List<CommonTraitLanguageDto>> NamedTraits { get; set; }
    }
}
