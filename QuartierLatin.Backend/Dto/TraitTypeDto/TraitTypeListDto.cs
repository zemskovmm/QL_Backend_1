using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.TraitTypeDto
{
    public class TraitTypeListDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("names")]
        public JObject Names { get; set; }
        [JsonProperty("identifier")]
        public string? Identifier { get; set; }
    }
}
