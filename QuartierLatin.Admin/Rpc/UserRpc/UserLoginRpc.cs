using System;
using System.Threading.Tasks;
using QuartierLatin.Admin.Auth;
using QuartierLatin.Admin.Config;
using QuartierLatin.Admin.Models;
using QuartierLatin.Admin.Models.Repositories;
using QuartierLatin.Admin.Utils;
using Microsoft.Extensions.Options;

namespace QuartierLatin.Admin.Rpc.UserRpc
{
    public class UserLoginRpc : LoginRpcBase<User, UserAuthManager>
    {
        private readonly IOptions<AzureAuthConfiguration> _azureConfig;
        private readonly UserAuthManager _mgr;
        private readonly IUserRepository _userManager;
        private readonly IEmailConfirmationService _confirmation;

        public UserLoginRpc(UserAuthManager mgr, IOptions<AzureAuthConfiguration> azureConfig,
            IEmailConfirmationService confirmation, IUserRepository userManager) : base(mgr,
            "X-User-Auth")
        {
            _mgr = mgr;
            _azureConfig = azureConfig;
            _confirmation = confirmation;
            _userManager = userManager;
        }

        public async Task<Result> Register(string email, string password)
        {
            var res = _mgr.Register(email, "", Guid.Empty, password);
            if (res.Success is not true) return res;

            await _confirmation.SendNewConfirmationCode(res.Value.Id);
            return res;
        }

        public async Task<Result> ConfirmAccount(string code)
        {
            try
            {
                await _confirmation.ConfirmUser(code);
                return Result.Succeeded;
            }
            catch (Exception e)
            {
                return ErrorCode.UserNotFound;
            }
        }

        public string AzureAuthUrl()
        {
            return _mgr.AzureRedirectUrl();
        }

        public string AzureLogoutUrl()
        {
            return _mgr.AzureLogoutUrl(_azureConfig.Value.LogoutRedirectUrl);
        }

        public string AzureReloginUrl()
        {
            return _mgr.AzureReloginUrl();
        }
    }
}