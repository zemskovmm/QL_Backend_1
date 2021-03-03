using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using QuartierLatin.Admin.Auth;
using QuartierLatin.Admin.Dto;
using QuartierLatin.Admin.Models;
using QuartierLatin.Admin.Models.Repositories;

namespace QuartierLatin.Admin.Application
{
    public interface IUserAppService
    {
        Task RegisterUser(UserRegisterFormDto model, string role);
        UserProfileDto FindUser(string email);
        UserProfileDto Login(string email, string password);
    }

    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _user;
        private readonly IRoleRepository _role;
        private readonly IConfirmationService _confirmationService;

        public UserAppService(IUserRepository user, IConfirmationService confirmationService, IRoleRepository role)
        {
            _user = user;
            _confirmationService = confirmationService;
            _role = role;
        }

        public async Task RegisterUser(UserRegisterFormDto model, string role)
        {
            var (email, password, name) = model;

            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"[\w.-]+@.+\.[a-z]{2,3}"))
                throw new ArgumentException("Invalid email");

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                throw new ArgumentException("Weak password");

            var id = _user.Create(email, Guid.Empty, name, PasswordToolkit.EncodeSshaPassword(password));
            await _role.AttachRole(new UserRole
            {
                Role = role,
                UserId = id
            });
            await _confirmationService.SendNewConfirmationCode(id);
        }

        public UserProfileDto FindUser(string email)
        {
            var user = _user.FindByLogin(email);
            if (user is null)
                throw new ArgumentException("User not found");
            
            return UserProfileDto.FromUser(user);
        }

        public UserProfileDto Login(string email, string password)
        {
            var user = _user.FindByLogin(email);
            if (user is null)
                throw new ArgumentException("Wrong password");
            if (!PasswordToolkit.CheckPassword(user.PasswordHash, password))
                throw new ArgumentException("Wrong password");
            
            return UserProfileDto.FromUser(user);
        }
    }
}