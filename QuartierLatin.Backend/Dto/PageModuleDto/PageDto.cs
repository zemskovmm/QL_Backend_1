using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace QuartierLatin.Backend.Dto.PageModuleDto
{
    public class PageDto
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("block")]
        public IEnumerable<PageBlockDto> PageBlockDto { get; set; }

        public PageDto(string title, IEnumerable<PageBlockDto> pageBlockDto)
        {
            Title = title;
            PageBlockDto = pageBlockDto;
        }
    }
}
