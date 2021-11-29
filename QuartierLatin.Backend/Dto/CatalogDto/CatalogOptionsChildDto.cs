using Newtonsoft.Json;

namespace QuartierLatin.Backend.Dto 
{
    public class CatalogOptionsChildDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

    }
}
