using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces.PortalServices;
using QuartierLatin.Backend.Dto.PortalDto;
using QuartierLatin.Backend.Models.Constants;
using QuartierLatin.Backend.Models.Portal;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace QuartierLatin.Backend.Controllers.PortalControllers
{
    [Route("/api/portal")]
    public class PortalController : Controller
    {
        private readonly IPortalUserAppService _portalUserAppService;

        public PortalController(IPortalUserAppService portalUserAppService)
        {
            _portalUserAppService = portalUserAppService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody]PortalRegisterDto portalRegister)
        {
            var portalUserId = await _portalUserAppService.RegisterAsync(portalRegister.FirstName,
                portalRegister.LastName, portalRegister.Phone, portalRegister.Email, portalRegister.Password);

            if (portalUserId is 0)
                return new BadRequestObjectResult("Пользователь с таким email существует");

            var user = await _portalUserAppService.GetPortalUserByIdAsync(portalUserId);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("sub", user.Id.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationPortal.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                IsPersistent = true,
                IssuedUtc = DateTimeOffset.Now
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationPortal.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] PortalLoginModel portalLogin)
        {
            var user = new PortalUser();

            try
            {
                user = await _portalUserAppService.LoginAsync(portalLogin.Email, portalLogin.Password);
            }
            catch (Exception e)
            {
                return Forbid();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("sub", user.Id.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationPortal.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                IsPersistent = true,
                IssuedUtc = DateTimeOffset.Now
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationPortal.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok();
        }

        [HttpGet("logout")]
        public async Task<IActionResult> AdminLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationPortal.AuthenticationScheme);
            return Redirect("/");
        }
    }
}
