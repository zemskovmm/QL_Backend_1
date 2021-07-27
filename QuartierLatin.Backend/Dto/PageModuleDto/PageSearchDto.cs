using System;
using Newtonsoft.Json;
using QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto;
using System.Collections.Generic;
using QuartierLatin.Backend.Models.Enums;

namespace QuartierLatin.Backend.Dto.PageModuleDto
{
    public class PageSearchDto
    {
        [JsonProperty("type")]
        public PageType PageType { get; set; }
        [JsonProperty("pageSize")]
        public int? PageSize { get; set; }
        [JsonProperty("page")]
        public int PageNumber { get; set; }
        [JsonProperty("filters")]
        public List<CatalogSearchFilterDto> Filters { get; set; }
    }
}
