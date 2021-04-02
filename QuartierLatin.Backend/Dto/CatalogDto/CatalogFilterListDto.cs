using Newtonsoft.Json;
using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.CatalogDto
{
    public class CatalogFilterListDto
    {
        [JsonProperty("filters")]
        public List<CatalogFilterDto> Filters { get; set; }
    }
}
