using Newtonsoft.Json;
using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto
{
    public class CatalogSearchFilterDto
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; }
        [JsonProperty("values")]
        public List<int> Values { get; set; }
    }
}
