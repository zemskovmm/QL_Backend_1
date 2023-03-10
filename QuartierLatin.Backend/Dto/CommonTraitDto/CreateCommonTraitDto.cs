using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.CommonTraitDto
{
    public class CreateCommonTraitDto
    {
        [JsonProperty("names")]
        public Dictionary<string, string> Names { get; set; }
        [JsonProperty("iconId")]
        public int? IconBlobId { get; set; }
        [JsonProperty("order")]
        public int Order { get; set; }
        [JsonProperty("parentId")]
        public int? ParentId { get; set; }
    }
}
