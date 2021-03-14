using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.PageModuleDto
{
    public class PageBlockDto
    {
        [JsonProperty("type")]
        public string TypeName { get; set; }

        [JsonProperty("data")]
        public JObject PageData { get; set; }

        public PageBlockDto(string typeName, JObject pageData)
        {
            TypeName = typeName;
            PageData = pageData;
        }
    }
}
