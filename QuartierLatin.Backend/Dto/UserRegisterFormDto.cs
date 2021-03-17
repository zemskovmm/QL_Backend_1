using QuartierLatin.Backend.Models;

namespace QuartierLatin.Backend.Dto
{
    public class UserRegisterFormDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        public Admin ToUser(int id, string passwordHash)
        {
            return new()
            {
                Id = id,
                Email = Email,
                PasswordHash = passwordHash
            };
        }
        public void Deconstruct(out string email, out string password, out string name)
        {
            email = Email;
            password = Password;
            name = Name;
        }
    }
}