using Newtonsoft.Json.Linq;
using RemoteUi;

namespace QuartierLatin.Backend.Dto.CourseCatalogDto.Course
{
    public class CourseLanguageAdminDto
    {
        [RemoteUiField("Name")]
        public string Name { get; set; }
        [RemoteUiField("HtmlDescription")]
        public string HtmlDescription { get; set; }
        [RemoteUiField("Url")]
        public string Url { get; set; }
        public JObject? Metadata { get; set; }
    }
}
