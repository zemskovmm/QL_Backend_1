using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.AdminPageModuleDto
{
    public class PageListItemDto
    {
        public int Id { get; set; }
        public Dictionary<string, string> Urls { get; set; }
        public Dictionary<string, string> Titles { get; set; }
        public Dictionary<string, int?> PreviewImages { get; set; }
        
        public Dictionary<string, int?> SmallPreviewImages { get; set; }
        public Dictionary<string, int?> WidePreviewImages { get; set; }

        public Dictionary<string, JObject?> Metadata { get; set; }
        public Dictionary<string, DateTime?> Date { get; set; }
    }
}
