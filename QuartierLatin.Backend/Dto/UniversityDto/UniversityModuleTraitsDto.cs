using QuartierLatin.Backend.Dto.CommonTraitDto;
using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.UniversityDto
{
    public class UniversityModuleTraitsDto
    {
        public Dictionary<string, List<CommonTraitLanguageDto>> NamedTraits { get; set; }
        public List<UniversitySpecialtiesDto> UniversitySpecialties { get; set; }
        public List<UniversityDegreeDto> UniversityDegrees { get; set; }
    }
}
