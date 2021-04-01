using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.CommonTraitDto
{
    public class CreateCommonTraitDto
    {
        [JsonProperty("names")]
        public JObject Names { get; set; }
        [JsonProperty("iconId")]
        public long? IconBlobId { get; set; }
        [JsonProperty("order")]
        public int Order { get; set; }
        [JsonProperty("parentId")]
        public int? ParentId { get; set; }
    }
}
