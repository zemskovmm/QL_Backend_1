using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Enums;

namespace QuartierLatin.Backend.Dto.PortalApplicationDto
{
    public class UpdatePortalApplicationDto
    {
        public ApplicationType Type { get; set; }
        public int EntityId { get; set; }
        public JObject? CommonApplicationInfo { get; set; }
        public JObject? EntityTypeSpecificApplicationInfo { get; set; }
    }
}
