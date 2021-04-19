using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Dto.UniversityDto.GetUniversityListDto
{
    public class UniversityListDto
    { 
        public int Id { get; set; }
        public string Website { get; set; }
        public int? FoundationYear { get; set; }
        public int? MinimumAge { get; set; }
        public Dictionary<string, UniversityLanguageDto> Languages { get; set; }
    }
}
