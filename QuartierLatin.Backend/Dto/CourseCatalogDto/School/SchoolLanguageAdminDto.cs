using Newtonsoft.Json.Linq;
using RemoteUi;

namespace QuartierLatin.Backend.Dto.CourseCatalogDto.School
{
    public class SchoolLanguageAdminDto
    {
        [RemoteUiField("name")]
        public string Name { get; set; }
        [RemoteUiField("htmlDescription")]
        public string HtmlDescription { get; set; }
        [RemoteUiField("url")]
        public string Url { get; set; }
        // [RemoteUiField("metadata", "", RemoteUiFieldType.TextArea)]
        public JObject? Metadata { get; set; }
    }
}
