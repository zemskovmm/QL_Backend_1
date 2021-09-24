using LinqToDB;
using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models.Portal
{
    [Table("PortalUsers")]
    public class PortalUser : BaseModel
    {
        [Column] public string Email { get; set; }
        [Column] public string? Phone { get; set; }
        [Column] public string? FirstName { get; set; }
        [Column] public string? LastName { get; set; }
        [Column] public string PasswordHash { get; set; }
        [Column(DataType = DataType.BinaryJson)] public string? PersonalInfo { get; set; }
    }
}
