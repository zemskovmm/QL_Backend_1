using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Dto.PortalDto
{
    public class PortalUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public JObject? PersonalInfo { get; set; }
    }
}
