using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.AdminPageModuleDto
{
    public class AdminPageBlockDto
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("data")]
        public Dictionary<string,JObject> Data { get; set; }

        public AdminPageBlockDto(string type, Dictionary<string, JObject> data)
        {
            Type = type;
            Data = data;
        }
    }
}
