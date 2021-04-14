using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.TraitTypeDto
{
    public class TraitTypeDto
    {
        [JsonProperty("names")]
        public Dictionary<string, string> Names { get; set; }
        [JsonProperty("identifier")]
        public string? Identifier { get; set; }
    }
}
