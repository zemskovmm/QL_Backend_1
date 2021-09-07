using QuartierLatin.Backend.Models.Enums;

namespace QuartierLatin.Backend.Models.Portal
{
    public class PortalApplication : BaseModel
    {
        public int UserId { get; set; }
        public ApplicationStatus Status { get; set; }
        public ApplicationType? Type { get; set; }
        public int? EntityId { get; set; }
        public string? CommonTypeSpecificApplicationInfo { get; set; }
        public string? EntityTypeSpecificApplicationInfo { get; set; }
    }
}
