using QuartierLatin.Backend.Dto.UniversityDto.GetUniversityListDto;
using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.UniversityDto
{
    public class CreateUniversityDtoAdmin
    {
        public int? FoundationYear { get; set; }
        public int? LogoId { get; set; }
        public int? BannerId { get; set; }
        public Dictionary<string, UniversityLanguageDto> Languages { get; set; }
    }
}
