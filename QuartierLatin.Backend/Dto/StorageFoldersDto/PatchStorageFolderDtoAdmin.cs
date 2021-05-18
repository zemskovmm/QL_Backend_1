using Newtonsoft.Json;

namespace QuartierLatin.Backend.Dto.StorageFoldersDto
{
    public class PatchStorageFolderDtoAdmin
    {
        [JsonProperty("title")]
        public string StorageFolderName { get; set; }
    }
}
