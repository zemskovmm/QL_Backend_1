using Newtonsoft.Json;

namespace QuartierLatin.Backend.Dto.CatalogDto
{
    public class CatalogOptionsDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
