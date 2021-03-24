using Newtonsoft.Json;

namespace QuartierLatin.Backend.Dto.AdminPageModuleDto
{
    public class AdminPageModuleDto
    {
        [JsonProperty("pageRootId")]
        public int PageRootId { get; set; }
        [JsonProperty("page")]
        public AdminPageDto AdminPageDto { get; set; }

        public AdminPageModuleDto(AdminPageDto adminPageDto, int pageRootId)
        {
            AdminPageDto = adminPageDto;
            PageRootId = pageRootId;
        }
    }
}
