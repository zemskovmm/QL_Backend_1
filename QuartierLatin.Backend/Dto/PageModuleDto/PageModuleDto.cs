using Newtonsoft.Json;

namespace QuartierLatin.Backend.Dto.PageModuleDto
{
    public class PageModuleDto
    {
        [JsonProperty("page")]
        public PageDto PageDto { get; set; }
    }
}
