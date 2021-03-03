using QuartierLatin.Admin.Models;

namespace QuartierLatin.Admin.Dto
{
    public class UserRoleDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }

        public static UserRoleDto FromModel(UserRole role)
        {
            return new()
            {
                Id = role.Id,
                UserId = role.UserId,
                Role = role.Role
            };
        }
    }
}