using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.UniversityDto.GetUniversityListDto
{
    public class UniversityListDto
    { 
        public int Id { get; set; }
        public string Website { get; set; }
        public int? FoundationYear { get; set; }
        public int? LogoId { get; set; }
        public int? BannerId { get; set; }
        public int? MinimumAge { get; set; }
        public Dictionary<string, UniversityLanguageDto> Languages { get; set; }
        public List<int> GalleryList { get; set; }
    }
}
