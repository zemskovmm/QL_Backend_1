using Newtonsoft.Json;
using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto
{
    public class CatalogSearchDto
    {
        [JsonProperty("pageSize")]
        public int? PageSize { get; set; }
        [JsonProperty("page")]
        public int PageNumber { get; set; }
        [JsonProperty("filters")]
        public List<CatalogSearchFilterDto> Filters { get; set; }
    }
}
