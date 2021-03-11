using System;
using Newtonsoft.Json;

namespace QuartierLatin.Backend.Dto.PageModuleDto
{
    public class PageDto
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("block")]
        public PageBlockDto PageBlockDto { get; set; }
    }
}
