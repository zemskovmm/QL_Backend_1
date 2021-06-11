using QuartierLatin.Backend.Dto.CommonTraitDto;
using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.CurseCatalogDto.Curse.ModuleDto
{
    public class CurseModuleTraitsDto
    {
        public Dictionary<string, List<CommonTraitLanguageDto>> NamedTraits { get; set; }
    }
}
