using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.PortalRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PortalServices;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Portal;
using QuartierLatin.Backend.Managers.Auth;

namespace QuartierLatin.Backend.Services.PortalServices
{
    public class PortalUserAppService : IPortalUserAppService
    {
        private readonly IPortalUserRepository _portalUserRepository;
        public PortalUserAppService(IPortalUserRepository portalUserRepository)
        {
            _portalUserRepository = portalUserRepository;
        }

        public async Task<int> RegisterAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"[\w.-]+@.+\.[a-z]{2,3}"))
                throw new ArgumentException("Invalid email");

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                throw new ArgumentException("Weak password");

            return await _portalUserRepository.RegisterAsync(email, PasswordToolkit.EncodeSshaPassword(password));
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

        public async Task UpdateUserInfoAsync(int portalUserId, string? firstName, string? lastName, string? phone, JObject? personalInfo)
        {
            if(!string.IsNullOrWhiteSpace(phone) && !Regex.IsMatch(phone, @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"))
                throw new ArgumentException("Invalid phone");

            await _portalUserRepository.UpdatePortalUserInfo(portalUserId, firstName, lastName, phone, personalInfo);
        }

        public async Task<(int totalItems, List<PortalUser> users)> GetPortalUserAdminListAsync(string? firstName, string? lastName, string? email, string? phone, int page,
            int pageSize)
        {
            return await _portalUserRepository.GetPortalUserAdminListAsync(firstName, lastName, email, phone,
                pageSize * page, pageSize);
        }
    }
}
