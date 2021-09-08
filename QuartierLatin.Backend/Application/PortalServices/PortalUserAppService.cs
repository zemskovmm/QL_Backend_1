using QuartierLatin.Backend.Application.Interfaces.PortalServices;
using QuartierLatin.Backend.Auth;
using QuartierLatin.Backend.Models.Portal;
using QuartierLatin.Backend.Models.Repositories.PortalRepository;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Application.PortalServices
{
    public class PortalUserAppService : IPortalUserAppService
    {
        private readonly IPortalUserRepository _portalUserRepository;
        public PortalUserAppService(IPortalUserRepository portalUserRepository)
        {
            _portalUserRepository = portalUserRepository;
        }

        public async Task<int> RegisterAsync(string firstName, string lastName, string phone, string email, string password, JObject personalInfo)
        {
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"[\w.-]+@.+\.[a-z]{2,3}"))
                throw new ArgumentException("Invalid email");

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                throw new ArgumentException("Weak password");

            if (string.IsNullOrWhiteSpace(phone) || !Regex.IsMatch(phone, @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"))
                throw new ArgumentException("Invalid phone");

            return await _portalUserRepository.RegisterAsync(firstName, lastName, phone, email, PasswordToolkit.EncodeSshaPassword(password), personalInfo);
        }

        public async Task<PortalUser> GetPortalUserByIdAsync(int userId)
        {
            return await _portalUserRepository.GetPortalUserByIdAsync(userId);
        }

        public async Task<PortalUser> LoginAsync(string email, string password)
        {
           var user = await _portalUserRepository.LoginAsync(email);

           if (user is null)
               throw new ArgumentException("Wrong password");

           if (!PasswordToolkit.CheckPassword(user.PasswordHash, password))
               throw new ArgumentException("Wrong password");

           return user;
        }
    }
}
