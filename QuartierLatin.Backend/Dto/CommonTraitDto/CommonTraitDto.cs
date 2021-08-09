using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteUi;

namespace QuartierLatin.Backend.Dto.CommonTraitDto
{
    public class CommonTraitDtoRemoteUI
    {
        [RemoteUiField("Common Trait Type ID")]
        public int TraitTypeId { get; set; }
        [RemoteUiField("Names", Type = RemoteUiFieldType.Custom, CustomType = "TraitLanguageDictionary")]
        public Dictionary<string, string> Names { get; set; }
        [RemoteUiField("Icon ID")]
        public int? IconId { get; set; }
        [RemoteUiField("Order")]
        public int Order { get; set; }
        [RemoteUiField("Parent Trait ID")]
        public int? ParentId { get; set; }
    }
    
    public class CommonTraitDto
    {
        [JsonProperty("traitTypeId")]
        public int CommonTraitTypeId { get; set; }
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
