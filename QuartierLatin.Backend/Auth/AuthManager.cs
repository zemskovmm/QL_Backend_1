using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using QuartierLatin.Backend.Dto;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Utils;
using Microsoft.Extensions.Logging;

namespace QuartierLatin.Backend.Auth
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

        public async Task<Result<(TUser user, string token)>> Login(string login, string password)
        {
            var user = _repo.FindByLogin(login);
            if (user == null)
                return ErrorCode.UserNotFound;
            if (!PasswordToolkit.CheckPassword(user.PasswordHash, password))
                return ErrorCode.InvalidPassword;
            var userRoles = (await _roleRepository.UserRolesByIds(user.Id))
                .Where(x => x.UserId == user.Id)
                .Select(x => x.Role)
                .ToList();

            var token = CreateToken(user, userRoles);

            return (user, token);
        }

        public abstract Task<Result<(User user, string token)>> LoginViaAzure(string code);

        private class AuthToken<T>
        {
            public int UserId { get; set; }
            public List<string> Roles { get; set; }
        }
    }

    public class UserAuthManager : AuthManager<User>
    {
        private readonly IAzureAdClient _azure;
        private readonly ILogger _logger;
        private readonly IUserRepository _repo;

        public UserAuthManager(ITokenStorage tokenStorage, IUserRepository repo, IAzureAdClient azure,
            IRoleRepository roleRepository, ILoggerFactory loggerFactory)
            : base(tokenStorage, repo, roleRepository)
        {
            _repo = repo;
            _azure = azure;
            _logger = loggerFactory.CreateLogger("UserAuthManager");
        }

        public override async Task<Result<(User user, string token)>> LoginViaAzure(string code)
        {
            var authResult = await _azure.GetTokenFromCode(code);
            var user = await _azure.GetUserInfo(authResult);
            return Result.Catch(() =>
            {
                var appUser = _repo.FindByLogin(user.Email);
                if (appUser is null)
                {
                    _repo.Create(user.Email, Guid.Parse(user.Id), user.Name, null);
                    appUser = _repo.FindByLogin(user.Email);
                }

                var token = CreateToken(appUser, user.Groups);
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

        public Result<UserProfileDto> Register(string email, string name, Guid identityId, string password)
        {
            email = email?.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"[\w.-]+@.+\.[a-z]{2,3}"))
                return ErrorCode.InvalidEmail;

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return ErrorCode.WeakPassword;
            return Result.Catch(() =>
            {
                var userId = _repo.Create(email, identityId, name, PasswordToolkit.EncodeSshaPassword(password));
                var registeredUser = new UserProfileDto
                {
                    Id = userId,
                    Email = email,
                    Name = name
                };
                return registeredUser;
            }, ErrorCode.EmailIsAlreadyRegistered, _logger);
        }
    }
}