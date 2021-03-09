using LinqToDB.Mapping;

namespace QuartierLatin.Backend.Models
{
    public static class Roles
    {
        public const string Admin = "Admin";

        public static readonly string[] ValidRolesList =
        {
            Admin
        };
    }

    public class UserRole
    {
        [PrimaryKey] [Identity] public int Id { get; set; }

        [Column] public int UserId { get; set; }

        [Column] public string Role { get; set; }
    }
}