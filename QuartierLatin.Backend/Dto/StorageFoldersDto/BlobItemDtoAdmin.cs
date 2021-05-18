using Newtonsoft.Json;

namespace QuartierLatin.Backend.Dto.StorageFoldersDto
{
    public class BlobItemDtoAdmin : BaseDto
    {
        [JsonProperty("title")]
        public string OriginalFileName { get; set; }
    }
}
