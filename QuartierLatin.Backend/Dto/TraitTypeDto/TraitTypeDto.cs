using Newtonsoft.Json;
using System.Collections.Generic;
using RemoteUi;

namespace QuartierLatin.Backend.Dto.TraitTypeDto
{
    public class TraitTypeDto
    {
        [JsonProperty("names")]
        [RemoteUiField("Names", Type = RemoteUiFieldType.Custom, CustomType = "TraitLanguageDictionary")]
        public Dictionary<string, string> Names { get; set; }
        
        [JsonProperty("identifier")]
        [RemoteUiField("Identifier")]
        public string? Identifier { get; set; }
        [JsonProperty("order")]
        [RemoteUiField("Order")]
        public int Order { get; set; }
    }
}
