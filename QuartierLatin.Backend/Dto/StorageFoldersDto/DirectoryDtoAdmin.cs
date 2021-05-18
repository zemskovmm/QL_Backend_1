using Newtonsoft.Json;

namespace QuartierLatin.Backend.Dto.StorageFoldersDto
{
    public class DirectoryDtoAdmin : BaseDto
    {
        [JsonProperty("title")]
        public string DirectoryName { get; set; }
    }
}
