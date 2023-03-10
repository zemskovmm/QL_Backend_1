using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Dto.UniversityDto.GetUniversityListDto;

namespace QuartierLatin.Backend.Dto.UniversityDto
{
    public class UniversityDto
    {
        public int? FoundationYear { get; set; }
        public int? LogoId { get; set; }
        public int? BannerId { get; set; }
        public Dictionary<string, UniversityLanguageDto> Languages { get; set; }
        public List<int> GalleryList { get; set; }
    }
}
