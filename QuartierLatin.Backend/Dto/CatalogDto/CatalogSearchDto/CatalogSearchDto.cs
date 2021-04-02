using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto
{
    public class CatalogSearchDto
    {
        [JsonProperty("page")]
        public int PageNumber { get; set; }
        [JsonProperty("filters")]
        public List<CatalogSearchFilterDto> Filters { get; set; }
    }
}
