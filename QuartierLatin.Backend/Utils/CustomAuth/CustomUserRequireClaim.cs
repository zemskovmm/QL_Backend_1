using Microsoft.AspNetCore.Authorization;

namespace QuartierLatin.Backend.Utils.CustomAuth
{
    public class CustomUserRequireClaim : IAuthorizationRequirement
    {
        public string ClaimType { get; }
        public string ClaimValue { get; }
        public CustomUserRequireClaim(string claimType, string claimValue)
        {
            ClaimType = claimType;
            ClaimValue = claimValue;
        }
    }
}
