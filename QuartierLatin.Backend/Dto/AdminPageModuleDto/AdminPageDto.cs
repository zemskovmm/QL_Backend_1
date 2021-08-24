using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.AdminPageModuleDto
{
    public class AdminPageDto
    {
        [JsonProperty("title")]
        public Dictionary<string, string> Title { get; set; }

        [JsonProperty("blocks")]
        public Dictionary<string, JObject> Blocks { get; set; }

        public List<DateTime?> Dates { get; set; }

        public Dictionary<string, JObject?> Metadata { get; set; }

        public AdminPageDto(Dictionary<string, string> title, Dictionary<string, JObject> adminPageBlockDtos,
            List<DateTime?> dates, Dictionary<string, JObject?> metadata)
        {
            Title = title;
            Blocks = adminPageBlockDtos;
            Dates = dates;
            Metadata = metadata;
        }
    }
}
