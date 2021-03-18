using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using QuartierLatin.Backend.Auth;
using QuartierLatin.Backend.Dto;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.Repositories;

namespace QuartierLatin.Backend.Application
{
    public interface IUserAppService
    {
        Task RegisterAdmin(AdminRegisterFormDto model, string role);
        AdminProfileDto FindAdmin(string email);
        AdminProfileDto Login(string email, string password);
    }

    public class UserAppService : IUserAppService
    {
        private readonly IAdminRepository _admin;
        private readonly IRoleRepository _role;
        private readonly IConfirmationService _confirmationService;

        public UserAppService(IAdminRepository admin, IConfirmationService confirmationService, IRoleRepository role)
        {
            _admin = admin;
            _confirmationService = confirmationService;
            _role = role;
        }

        public async Task RegisterAdmin(AdminRegisterFormDto model, string role)
        {
            var (email, password, name) = model;

            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"[\w.-]+@.+\.[a-z]{2,3}"))
                throw new ArgumentException("Invalid email");

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                throw new ArgumentException("Weak password");

            var id = _admin.Create(email, Guid.Empty, name, PasswordToolkit.EncodeSshaPassword(password));
            await _role.AttachRole(new AdminRole()
            {
                Role = role,
                AdminId = id
            });
            await _confirmationService.SendNewConfirmationCode(id);
        }

        public AdminProfileDto FindAdmin(string email)
        {
            var admin = _admin.FindByLogin(email);
            if (admin is null)
                throw new ArgumentException("User not found");
            
            return AdminProfileDto.FromAdmin(admin);
        }

        public AdminProfileDto Login(string email, string password)
        {
            var admin = _admin.FindByLogin(email);
            if (admin is null)
                throw new ArgumentException("Wrong password");
            if (!PasswordToolkit.CheckPassword(admin.PasswordHash, password))
                throw new ArgumentException("Wrong password");
            
            return AdminProfileDto.FromAdmin(admin);
        }
    }
}