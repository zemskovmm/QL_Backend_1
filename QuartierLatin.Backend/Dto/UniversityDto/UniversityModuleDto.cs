using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.UniversityDto
{
    public class UniversityModuleDto
    {
        public string Title { get; set; }

        public string DescriptionHtml { get; set; }
        
        public int? FoundationYear { get; set; }

        public UniversityModuleTraitsDto Traits { get; set; }
        public JObject? Metadata { get; set; }
    }
}
