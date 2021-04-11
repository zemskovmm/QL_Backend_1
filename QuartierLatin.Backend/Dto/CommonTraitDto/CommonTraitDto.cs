using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.CommonTraitDto
{
    public class CommonTraitDto
    {
        [JsonProperty("traitTypeId")]
        public int CommonTraitTypeId { get; set; }
        [JsonProperty("names")]
        public JObject Names { get; set; }
        [JsonProperty("iconId")]
        public int? IconBlobId { get; set; }
        [JsonProperty("order")]
        public int Order { get; set; }
        [JsonProperty("parentId")]
        public int? ParentId { get; set; }
    }
}
