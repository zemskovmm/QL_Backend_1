using QuartierLatin.Backend.Dto.CommonTraitDto;
using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.CurseCatalogDto.School.ModuleDto
{
    public class SchoolModuleTraitsDto
    {
        public Dictionary<string, List<CommonTraitLanguageDto>> NamedTraits { get; set; }
    }
}
