using Newtonsoft.Json;

namespace QuartierLatin.Backend.Dto.AdminPageModuleDto
{
    public class AdminPageModuleDto
    {
        [JsonProperty("page")]
        public AdminPageDto AdminPageDto { get; set; }

        public AdminPageModuleDto(AdminPageDto adminPageDto)
        {
            AdminPageDto = adminPageDto;
        }
    }
}
