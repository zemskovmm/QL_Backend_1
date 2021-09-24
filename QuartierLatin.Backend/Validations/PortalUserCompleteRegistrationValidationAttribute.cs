using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PortalServices;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Constants;
using System;
using System.Linq;

namespace QuartierLatin.Backend.Validations
{
    public class PortalUserCompleteRegistrationValidationAttribute : ActionFilterAttribute
    {
        private readonly IPortalUserAppService _portalUserAppService;

        public PortalUserCompleteRegistrationValidationAttribute(IPortalUserAppService portalUserAppService)
        {
            _portalUserAppService = portalUserAppService;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userClaims = filterContext.HttpContext.Request.HttpContext.User?.Identities?.FirstOrDefault(identity =>
                identity.AuthenticationType == CookieAuthenticationPortal.AuthenticationScheme)?.Claims;

            if (userClaims is null)
            {
                filterContext.Result = new BadRequestObjectResult("User not found");
            }
            else
            {
                var userId = Convert.ToInt32(userClaims.FirstOrDefault(claim => claim.Type == "sub").Value);

                var portalUser = _portalUserAppService.GetPortalUserByIdAsync(userId)
                    .ConfigureAwait(false)
                    .GetAwaiter().GetResult();

                if (portalUser.FirstName is null ||
                    portalUser.LastName is null ||
                    portalUser.Phone is null ||
                    portalUser.PersonalInfo is null)
                    filterContext.Result = new BadRequestObjectResult("User has not completed registration");
            }
        }
    }
}
