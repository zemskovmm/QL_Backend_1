using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Dto.UniversityDto.GetUniversityListDto;

namespace QuartierLatin.Backend.Dto.UniversityDto
{
    public class UniversityDto
    {
        public string Website { get; set; }
        public int? FoundationYear { get; set; }
        public int? MinimumAge { get; set; }
        public Dictionary<string, UniversityLanguageDto> Languages { get; set; }
    }
}
