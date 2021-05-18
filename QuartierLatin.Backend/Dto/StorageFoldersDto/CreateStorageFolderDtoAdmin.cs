using Newtonsoft.Json;

namespace QuartierLatin.Backend.Dto.StorageFoldersDto
{
    public class CreateStorageFolderDtoAdmin
    {
        [JsonProperty("title")]
        public string FolderName { get; set; }
        [JsonProperty("parentId")]
        public int? FolderParentId { get; set; }
    }
}
