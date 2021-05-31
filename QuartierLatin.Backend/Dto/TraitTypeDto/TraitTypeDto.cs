using Newtonsoft.Json;
using System.Collections.Generic;

namespace QuartierLatin.Backend.Dto.TraitTypeDto
{
    public class TraitTypeDto
    {
        [JsonProperty("names")]
        public Dictionary<string, string> Names { get; set; }
        [JsonProperty("identifier")]
        public string? Identifier { get; set; }
        [JsonProperty("order")]
        public int Order { get; set; }
    }
}
