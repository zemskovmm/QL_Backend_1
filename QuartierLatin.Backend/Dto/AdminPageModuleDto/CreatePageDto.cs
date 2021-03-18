using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.AdminPageModuleDto
{
    public class CreatePageDto
    {
        public string Url { get; set; }
        public string Title { get; set; }

        public JObject PageData { get; set; }
        public int LanguageId { get; set; }
        public int PageRootId { get; set; }
    }
}
