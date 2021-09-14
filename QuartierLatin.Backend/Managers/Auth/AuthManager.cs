using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Models;
using QuartierLatin.Backend.Dto;
using QuartierLatin.Backend.Utils;

namespace QuartierLatin.Backend.Managers.Auth
{
    public abstract class AuthManager<TUser> where TUser : IHaveId, IHavePasswordAuth
    {
        private readonly IPasswordAuthRepository<TUser> _repo;
        private readonly IRoleRepository _roleRepository;
        private readonly ITokenStorage _tokenStorage;

        public AuthManager(ITokenStorage tokenStorage, IPasswordAuthRepository<TUser> repo,
            IRoleRepository roleRepository)
        {
            _tokenStorage = tokenStorage;
            _repo = repo;
            _roleRepository = roleRepository;
        }

        public (TUser user, string renewToken)? Auth(string token)
        {
            var loaded = _tokenStorage.LoadToken<AuthToken<TUser>>(token);
            if (loaded is null) return null;

            return (_repo.GetById(loaded.Value.data.UserId), loaded.Value.token);
        }

        public List<string> Roles(string token)
        {
            var loaded = _tokenStorage.LoadToken<AuthToken<TUser>>(token);
            return loaded is null ? new List<string>() : loaded.Value.data.Roles;
        }

        protected string CreateToken(TUser user, List<string> roles)
        {
            return _tokenStorage.CreateToken(new AuthToken<TUser>
            {
                UserId = user.Id,
                Roles = roles
            });
        }

        public async Task<Result<(TUser admin, string token)>> Login(string login, string password)
        {
            var admin = _repo.FindByLogin(login);
            if (admin == null)
                return ErrorCode.UserNotFound;
            if (!PasswordToolkit.CheckPassword(admin.PasswordHash, password))
                return ErrorCode.InvalidPassword;
            var adminRoles = (await _roleRepository.UserRolesByIds(admin.Id))
                .Where(x => x.AdminId == admin.Id)
                .Select(x => x.Role)
                .ToList();

            var token = CreateToken(admin, adminRoles);

            return (admin, token);
        }

        public abstract Task<Result<(Admin admin, string token)>> LoginViaAzure(string code);

        private class AuthToken<T>
        {
            public int UserId { get; set; }
            public List<string> Roles { get; set; }
        }
    }

    public class UserAuthManager : AuthManager<Admin>
    {
        private readonly IAzureAdClient _azure;
        private readonly ILogger _logger;
        private readonly IAdminRepository _repo;

        public UserAuthManager(ITokenStorage tokenStorage, IAdminRepository repo, IAzureAdClient azure,
            IRoleRepository roleRepository, ILoggerFactory loggerFactory)
            : base(tokenStorage, repo, roleRepository)
        {
            _repo = repo;
            _azure = azure;
            _logger = loggerFactory.CreateLogger("UserAuthManager");
        }

        public override async Task<Result<(Admin admin, string token)>> LoginViaAzure(string code)
        {
            var authResult = await _azure.GetTokenFromCode(code);
            var admin = await _azure.GetUserInfo(authResult);
            return Result.Catch(() =>
            {
                var appUser = _repo.FindByLogin(admin.Email);
                if (appUser is null)
                {
                    _repo.Create(admin.Email, Guid.Parse(admin.Id), admin.Name, null);
                    appUser = _repo.FindByLogin(admin.Email);
                }

                var token = CreateToken(appUser, admin.Groups);
                return (appUser, token);
            }, ErrorCode.AccessDenied, _logger);
        }

        public string AzureRedirectUrl()
        {
            return _azure.CreateLoginUrl();
        }

        public string AzureLogoutUrl(string path)
        {
            return _azure.GetLogoutUrl(path);
        }

        public string AzureReloginUrl()
        {
            return _azure.GetReloginUrl();
        }

        public Result<AdminProfileDto> Register(string email, string name, Guid identityId, string password)
        {
            email = email?.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"[\w.-]+@.+\.[a-z]{2,3}"))
                return ErrorCode.InvalidEmail;

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return ErrorCode.WeakPassword;
            return Result.Catch(() =>
            {
                var adminId = _repo.Create(email, identityId, name, PasswordToolkit.EncodeSshaPassword(password));
                var registeredAdmin = new AdminProfileDto()
                {
                    Id = adminId,
                    Email = email,
                    Name = name
                };
                return registeredAdmin;
            }, ErrorCode.EmailIsAlreadyRegistered, _logger);
        }
    }
}