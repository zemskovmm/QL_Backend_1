using Newtonsoft.Json;
using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.CatalogDto
{
    public class CatalogFilterDto
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("options")]
        public List<CatalogOptionsDto> Options { get; set; } 
    }
}
