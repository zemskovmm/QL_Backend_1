using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CoreRPC;
using QuartierLatin.Admin.Models.Repositories;
using CoreRPC.Transferable;
using Microsoft.AspNetCore.Http;

namespace QuartierLatin.Admin.Auth.Requirements
{
    public class RpcRoleInterceptor : IMethodCallInterceptor
    {
        private readonly UserAuthManager _authManager;
        private readonly IRoleRepository _repository;

        public RpcRoleInterceptor(IRoleRepository repository, UserAuthManager authManager)
        {
            _repository = repository;
            _authManager = authManager;
        }

        public async Task<object> Intercept(MethodCall call, object context, Func<Task<object>> invoke)
        {
            var roleAttribute = call.Method.GetCustomAttributes().OfType<RpcRole>().FirstOrDefault();
            if (roleAttribute != null) CheckRoles(context as HttpContext, roleAttribute.Roles);
            return await invoke();
        }

        private void CheckRoles(HttpContext httpContext, IEnumerable<string> rolesToCheck)
        {
            var roles = _authManager.Roles(httpContext.Request.Headers["X-User-Auth"].FirstOrDefault());
            var hasRole = roles.Any(rolesToCheck.Contains);
            if (!hasRole) throw new UnauthorizedAccessException();
        }
    }

    public class RpcRole : Attribute
    {
        public readonly string[] Roles;

        public RpcRole(params string[] roles)
        {
            Roles = roles;
        }
    }
}