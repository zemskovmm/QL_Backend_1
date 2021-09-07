using LinqToDB.Mapping;
using QuartierLatin.Backend.Models.Enums;

namespace QuartierLatin.Backend.Models.Portal
{
    [Table("PortalApplications")]
    public class PortalApplication : BaseModel
    {
        [Column] public int UserId { get; set; }
        [Column] public ApplicationStatus Status { get; set; }
        [Column] public ApplicationType? Type { get; set; }
        [Column] public int? EntityId { get; set; }
        [Column] public string CommonTypeSpecificApplicationInfo { get; set; }
        [Column] public string EntityTypeSpecificApplicationInfo { get; set; }
    }
}
