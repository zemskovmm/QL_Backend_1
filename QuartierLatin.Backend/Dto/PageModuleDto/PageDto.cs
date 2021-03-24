using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.PageModuleDto
{
    public class PageDto
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("block")]
        public JObject PageBlockDto { get; set; }

        public PageDto(string title, JObject pageBlockDto)
        {
            Title = title;
            PageBlockDto = pageBlockDto;
        }
    }
}
