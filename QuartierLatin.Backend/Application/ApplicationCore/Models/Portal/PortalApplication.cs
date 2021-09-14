using LinqToDB;
using LinqToDB.Mapping;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.Portal
{
    [Table("PortalApplications")]
    public class PortalApplication : BaseModel
    {
        [Column] public int UserId { get; set; }
        [Column] public ApplicationStatus Status { get; set; }
        [Column] public ApplicationType? Type { get; set; }
        [Column] public int? EntityId { get; set; }
        [Column(DataType = DataType.BinaryJson)] public string CommonTypeSpecificApplicationInfo { get; set; }
        [Column(DataType = DataType.BinaryJson)] public string EntityTypeSpecificApplicationInfo { get; set; }
    }
}
