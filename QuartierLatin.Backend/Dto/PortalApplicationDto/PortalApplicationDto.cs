using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Enums;

namespace QuartierLatin.Backend.Dto.PortalApplicationDto
{
    public class PortalApplicationDto : BaseDto
    {
        public ApplicationType? Type { get; set; }
        public int? EntityId { get; set; }
        public ApplicationStatus Status { get; set; }
        public JObject? CommonApplicationInfo { get; set; }
        public JObject? EntityTypeSpecificApplicationInfo { get; set; }
    }
}
