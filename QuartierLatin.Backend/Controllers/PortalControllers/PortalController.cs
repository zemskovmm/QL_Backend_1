using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PortalServices;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Constants;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Portal;
using QuartierLatin.Backend.Dto.PortalDto;

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

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody]PortalRegisterDto portalRegister)
        {
            var portalUserId = await _portalUserAppService.RegisterAsync(portalRegister.Email, portalRegister.Password);

            if (portalUserId is 0)
                return new BadRequestObjectResult("Пользователь с таким email существует");

            var user = await _portalUserAppService.GetPortalUserByIdAsync(portalUserId);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FirstName ?? ""),
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

        [Authorize(AuthenticationSchemes = CookieAuthenticationPortal.AuthenticationScheme)]
        [HttpPost("heartbeat")]
        public async Task<IActionResult> CheckIsValidUserSession()
        {
            return Ok();
        }

        [AllowAnonymous]
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

            if (user is null)
                return Forbid();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FirstName ?? "" ),
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

        [Authorize(AuthenticationSchemes = CookieAuthenticationPortal.AuthenticationScheme)]
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationPortal.AuthenticationScheme);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationPortal.AuthenticationScheme)]
        [HttpPut("user")]
        public async Task<IActionResult> UpdatePortalUser([FromBody] PortalUserDto portalUserDto)
        {
            var userId = GetUserId();

            await _portalUserAppService.UpdateUserInfoAsync(userId, portalUserDto.FirstName, portalUserDto.LastName,
                portalUserDto.Phone, portalUserDto.PersonalInfo);

            return Ok();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationPortal.AuthenticationScheme)]
        [HttpGet("user"),
         ProducesResponseType(typeof(PortalUserDto), 200)]
        public async Task<IActionResult> GetPortalUser()
        {
            var userId = GetUserId();

            var portalUser = await _portalUserAppService.GetPortalUserByIdAsync(userId);

            var response = new PortalUserDto
            {
                FirstName = portalUser.FirstName,
                LastName = portalUser.LastName,
                PersonalInfo = portalUser.PersonalInfo is null ? null : JObject.Parse(portalUser.PersonalInfo),
                Phone = portalUser.Phone,
                Email = portalUser.Email
            };

            return Ok(response);
        }

        private int GetUserId()
        {
            var userClaims = User?.Identities?.FirstOrDefault(identity =>
                identity.AuthenticationType == CookieAuthenticationPortal.AuthenticationScheme)?.Claims;

            if (userClaims is null)
                throw new ArgumentNullException("User is null");

            var userId = Convert.ToInt32(userClaims.FirstOrDefault(claim => claim.Type == "sub").Value);
            return userId;
        }
    }
}
