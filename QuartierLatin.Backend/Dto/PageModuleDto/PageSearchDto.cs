using System;
using Newtonsoft.Json;
using QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto;
using System.Collections.Generic;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;

namespace QuartierLatin.Backend.Dto.PageModuleDto
{
    public class PageSearchDto
    {
        [JsonProperty("pageType")]
        public PageType PageType { get; set; }
        [JsonProperty("pageSize")]
        public int? PageSize { get; set; }
        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; }
        [JsonProperty("filters")]
        public List<CatalogSearchFilterDto> Filters { get; set; }
    }
}
