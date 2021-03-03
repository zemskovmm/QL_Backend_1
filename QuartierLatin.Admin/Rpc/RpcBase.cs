using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreRPC.AspNetCore;
using QuartierLatin.Admin.Auth;
using QuartierLatin.Admin.Models;
using QuartierLatin.Admin.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace QuartierLatin.Admin.Rpc
{
    public abstract class RpcBase : IHttpContextAwareRpc
    {
        protected HttpContext HttpContext { get; private set; }

        Task<object> IHttpContextAwareRpc.OnExecuteRpcCall(HttpContext context, Func<Task<object>> action)
        {
            HttpContext = context;
            return OnRpcCall(context, action);
        }

        protected virtual Task<object> OnRpcCall(HttpContext context, Func<Task<object>> action)
        {
            return action();
        }
    }

    public abstract class AuthorizedRpcBase<TUser, TAuthManager> : RpcBase
        where TAuthManager : AuthManager<TUser>
        where TUser : IHaveId, IHavePasswordAuth
    {
        private readonly string _headerName;

        public AuthorizedRpcBase(string headerName)
        {
            _headerName = headerName;
        }

        protected TUser User { get; private set; }

        protected List<string> UserRoles { get; private set; }

        private Task<object> ReturnNotAuthorized(HttpContext context)
        {
            context.Response.StatusCode = 401;
            return Task.FromResult((object) new
            {
                Error = "Not authorized"
            });
        }

        protected override Task<object> OnRpcCall(HttpContext context, Func<Task<object>> action)
        {
            var authManager = context.RequestServices.GetRequiredService<TAuthManager>();
            var hdr = context.Request.Headers[_headerName].FirstOrDefault();
            /*if (hdr == null)
                return ReturnNotAuthorized(context);*/
            var res = authManager.Auth(hdr);
            if (res == null)
                return ReturnNotAuthorized(context);
            User = res.Value.user;
            UserRoles = authManager.Roles(hdr);
            return base.OnRpcCall(context, action);
        }
    }

    public abstract class UserRpcBase : AuthorizedRpcBase<User, UserAuthManager>
    {
        public UserRpcBase() : base("X-User-Auth")
        {
        }

        protected bool HasRole(string role)
        {
            return UserRoles.Contains(role);
        }

        protected bool HasAnyRoles(params string[] roles)
        {
            foreach (var r in roles)
                if (HasRole(r))
                    return true;
            return false;
        }
    }

    public abstract class LoginRpcBase<TUser, TAuthManager> : RpcBase
        where TAuthManager : AuthManager<TUser>
        where TUser : IHaveId, IHavePasswordAuth
    {
        private readonly string _headerName;
        private readonly AuthManager<TUser> _mgr;

        public LoginRpcBase(AuthManager<TUser> mgr, string headerName)
        {
            _mgr = mgr;
            _headerName = headerName;
        }

        public async Task<Result<string>> Login(string username, string password)
        {
            return (await _mgr.Login(username, password)).Map(x => x.token);
        }

        public async Task<Result<string>> LoginAdmin(string username, string password)
        {
            var res = await _mgr.Login(username, password);
            var tokenResponse = res.Map(x => x.token);

            if (!res.Success) return tokenResponse;
            var (user, token) = res.Value;

            return !_mgr.Roles(token).Contains(Roles.Admin) ? ErrorCode.InvalidPassword : tokenResponse;
        }

        public async Task<Result<string>> LoginViaAzureCode(string code)
        {
            return (await _mgr.LoginViaAzure(code)).Map(x => x.token);
        }

        public bool CheckAuthToken()
        {
            var authManager = HttpContext.RequestServices.GetRequiredService<TAuthManager>();
            var hdr = HttpContext.Request.Headers[_headerName].FirstOrDefault();
            if (hdr == null)
                return false;
            var res = authManager.Auth(hdr);
            return res.HasValue;
        }
    }
}