using System.Collections.Generic;
using Newtonsoft.Json;

namespace QuartierLatin.Backend.Dto.AdminPageModuleDto
{
    public class AdminPageDto
    {
        [JsonProperty("title")]
        public Dictionary<string, string> Title { get; set; }

        [JsonProperty("blocks")]
        public IEnumerable<AdminPageBlockDto> Blocks { get; set; }

        public AdminPageDto(Dictionary<string, string> title, IEnumerable<AdminPageBlockDto> adminPageBlockDtos)
        {
            Title = title;
            Blocks = adminPageBlockDtos;
        }
    }
}
