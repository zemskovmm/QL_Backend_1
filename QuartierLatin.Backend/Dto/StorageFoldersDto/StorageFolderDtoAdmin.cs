using System.Collections.Generic;
using Newtonsoft.Json;

namespace QuartierLatin.Backend.Dto.StorageFoldersDto
{
    public class StorageFolderDtoAdmin : BaseDto
    {
        [JsonProperty("title")]
        public string FolderName { get; set; }

        public int? ParentId { get; set; }

        [JsonProperty("directories")]
        public List<DirectoryDtoAdmin> Directories { get; set; }

        [JsonProperty("media")]
        public List<BlobItemDtoAdmin> Files { get; set; }
    }
}
