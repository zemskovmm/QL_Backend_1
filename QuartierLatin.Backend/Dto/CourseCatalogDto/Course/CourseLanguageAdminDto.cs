using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.CourseCatalogDto.Course
{
    public class CourseLanguageAdminDto
    {
        public string Name { get; set; }
        public string HtmlDescription { get; set; }
        public string Url { get; set; }
        public JObject? Metadata { get; set; }
    }
}
