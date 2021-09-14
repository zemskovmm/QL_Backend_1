using Newtonsoft.Json;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;

namespace QuartierLatin.Backend.Dto.AdminPageModuleDto
{
    public class AdminPageModuleDto
    {
        [JsonProperty("pageRootId")]
        public int PageRootId { get; set; }
        [JsonProperty("page")]
        public AdminPageDto AdminPageDto { get; set; }
        public PageType PageType { get; set; }

        public AdminPageModuleDto(AdminPageDto adminPageDto, int pageRootId, PageType pageType)
        {
            AdminPageDto = adminPageDto;
            PageRootId = pageRootId;
            PageType = pageType;
        }
    }
}
