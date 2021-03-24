﻿using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.AdminPageModuleDto
{
    public class PageDto
    {
        public string Url { get; set; }
        public string Title { get; set; }

        public JObject PageData { get; set; }

    }
}
