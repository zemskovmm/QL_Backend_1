using Newtonsoft.Json;

namespace QuartierLatin.Backend.Dto.CommonTraitDto
{
    public class CommonTraitLanguageDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("identifier")]
        public string? Identifier { get; set; }
        [JsonProperty("iconId")]
        public int? IconBlobId { get; set; }
    }
}
