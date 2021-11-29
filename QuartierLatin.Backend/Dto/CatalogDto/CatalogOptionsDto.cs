using Newtonsoft.Json;
using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.CatalogDto
{
    public class CatalogOptionsDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
		[JsonProperty("items")]
        public List<CatalogOptionsChildDto> Items { get; set; } 
    }
}
