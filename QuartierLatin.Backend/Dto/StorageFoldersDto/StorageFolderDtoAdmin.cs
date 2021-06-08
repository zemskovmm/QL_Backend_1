using System.Collections.Generic;
using Newtonsoft.Json;

namespace QuartierLatin.Backend.Dto.StorageFoldersDto
{
    public class StorageFolderDtoAdmin
    {
        public int? Id { get; set; }

        [JsonProperty("title")]
        public string FolderName { get; set; }

        public int? ParentId { get; set; }

        [JsonProperty("directories")]
        public List<DirectoryDtoAdmin> Directories { get; set; }

        [JsonProperty("media")]
        public List<BlobItemDtoAdmin> Files { get; set; }
    }
}
