using System;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.AdminPageModuleDto
{
    public class PageLanguageDto
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public JObject PageData { get; set; }
        public DateTime? Date { get; set; }
    }
}
