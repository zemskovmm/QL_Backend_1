using Microsoft.AspNetCore.Authorization;

namespace QuartierLatin.Backend.Utils.CustomAuth
{
    public static class AuthorizationPolicyBuilderExtension
    {
        public static AuthorizationPolicyBuilder UserRequireCustomClaim(this AuthorizationPolicyBuilder builder, string claimType, string claimValue)
        {
            builder.AddRequirements(new CustomUserRequireClaim(claimType, claimValue));
            return builder;
        }
    }
}
