using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models
{
    public static class Roles
    {
        public const string Admin = "Admin";

        public static readonly string[] ValidRolesList =
        {
            Admin
        };
    }

    public class AdminRole
    {
        [PrimaryKey] [Identity] public int Id { get; set; }

        [Column] public int AdminId { get; set; }

        [Column] public string Role { get; set; }
    }
}