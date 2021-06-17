using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.UniversityDto
{
    public class UniversityModuleDto
    {
        public string Title { get; set; }

        public string DescriptionHtml { get; set; }
        
        public int? FoundationYear { get; set; }
        public int? LogoId { get; set; }
        public int? BannerId { get; set; }
        public UniversityModuleTraitsDto Traits { get; set; }
        public List<int> GalleryList { get; set; }
    }
}
