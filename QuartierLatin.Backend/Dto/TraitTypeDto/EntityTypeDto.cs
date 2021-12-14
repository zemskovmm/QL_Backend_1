using Newtonsoft.Json;
using System.Collections.Generic;
using RemoteUi;

namespace QuartierLatin.Backend.Dto.TraitTypeDto
{
    public class EntityTypeDto
    {
        
        [JsonProperty("entityTypeName")]
        [RemoteUiField("EntityTypeName")]
        public string? EntityTypeName { get; set; }
        
		[JsonProperty("entityTypeId")]
        [RemoteUiField("EntityTypeId")]
        public int EntityTypeId { get; set; }
		
		
    }
}
