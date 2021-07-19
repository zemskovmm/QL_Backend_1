using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.UniversityDto.GetUniversityListDto
{
    public class UniversityLanguageDto
    {
        public string Name { get; set; }
        public string HtmlDescription { get; set; }
        public string Url { get; set; }
        public JObject? Metadata { get; set; }
    }
}
