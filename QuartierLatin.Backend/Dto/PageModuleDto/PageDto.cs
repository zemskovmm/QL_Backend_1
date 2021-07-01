using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Enums;

namespace QuartierLatin.Backend.Dto.PageModuleDto
{
    public class PageDto
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("block")]
        public JObject PageBlockDto { get; set; }

        public DateTime? Date { get; set; }

        public PageType PageType { get; set; }

        public PageDto(string title, JObject pageBlockDto, DateTime? date, PageType pageType)
        {
            Title = title;
            PageBlockDto = pageBlockDto;
            Date = date;
            PageType = pageType;
        }
    }
}
