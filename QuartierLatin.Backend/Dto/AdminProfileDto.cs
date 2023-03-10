using QuartierLatin.Backend.Application.ApplicationCore.Models;

namespace QuartierLatin.Backend.Dto
{
    public class AdminProfileDto
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public string Name { get; set; }

        public int AvatarImage { get; set; }

        public static AdminProfileDto FromAdmin(Admin admin)
        {
            return new()
            {
                Id = admin.Id,
                Email = admin.Email,
                Name = admin.Name,
                AvatarImage = admin.AvatarImage
            };
        }
    }
}