using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;

namespace QuartierLatin.Backend.Dto.PortalApplicationDto
{
    public class PortalApplicationWithoutIdDto
    {
        public ApplicationType? Type { get; set; }
        public int? EntityId { get; set; }
        public JObject? CommonApplicationInfo { get; set; }
        public JObject? EntityTypeSpecificApplicationInfo { get; set; }
    }
}
