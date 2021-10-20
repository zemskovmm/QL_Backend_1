using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Utils.CustomAuth
{
    public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            RolesAuthorizationRequirement requirement)
        {
            if (context?.User?.Identity == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var validRole = false;
            if (requirement.AllowedRoles is null ||
                requirement.AllowedRoles.Any() is false)
            {
                validRole = true;
            }
            else
            {
                var claims = context.User.Claims;
                var userRole = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                var rolesAllowed = requirement.AllowedRoles;

                validRole = rolesAllowed.Any(role => role == userRole);
            }

            if (validRole)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
