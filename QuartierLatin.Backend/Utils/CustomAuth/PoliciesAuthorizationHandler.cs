using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Utils.CustomAuth
{
    public class PoliciesAuthorizationHandler : AuthorizationHandler<CustomUserRequireClaim>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CustomUserRequireClaim requirement)
        {
            if (context?.User?.Identity is null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var hasClaim = context.User.Claims.Any(x => x.Type == requirement.ClaimType);

            if (hasClaim)
            {
                var claimValue = context?.User?.Claims.FirstOrDefault(claim => claim.Type == requirement.ClaimType)?.Value;

                if (claimValue is not null && claimValue == requirement.ClaimValue)
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }

                context.Fail();
                return Task.CompletedTask;
            }

            context.Fail();
            return Task.CompletedTask;
        }
    }
}
