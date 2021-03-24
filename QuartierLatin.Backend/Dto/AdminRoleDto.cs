using QuartierLatin.Backend.Models;

namespace QuartierLatin.Backend.Dto
{
    public class AdminRoleDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }

        public static AdminRoleDto FromModel(AdminRole role)
        {
            return new()
            {
                Id = role.Id,
                UserId = role.AdminId,
                Role = role.Role
            };
        }
    }
}