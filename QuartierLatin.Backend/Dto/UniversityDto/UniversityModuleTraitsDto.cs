using QuartierLatin.Backend.Dto.TraitTypeDto;
using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.UniversityDto
{
    public class UniversityModuleTraitsDto
    {
        public List<TraitTypeLanguageDto> Cities { get; set; }
        public List<TraitTypeLanguageDto> Degrees { get; set; }
        public List<UniversityInstructionLanguageDto> InstructionLanguage { get; set; }

        public List<UniversitySpecialtiesDto> UniversitySpecialties { get; set; }
    }
}
