using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.ApplicationCore.Models;
using QuartierLatin.Backend.Dto;
using QuartierLatin.Backend.Services;

namespace QuartierLatin.Backend.Controllers
{
    public record AdminLoginModel(string Username, string Password, bool RememberMe);
    
    [Route("/api/admin/auth")]
    [AllowAnonymous]
    public class Auth : Controller
    {

        private readonly IUserAppService _user;
        private readonly IRolesAppService _roles;

        public Auth(IUserAppService user, IRolesAppService roles)
        {
            _user = user;
            _roles = roles;
        }

        // GET
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]AdminLoginModel model)
        {
            AdminProfileDto user;
            
            try
            { 
                user = _user.Login(model.Username, model.Password);
            }
            catch (Exception e)
            {
                return Forbid();
            }
            
            var roles = await _roles.GetUserRolesByIds(new List<int> {user.Id});
            
            if (!roles.Any(x => x.Role == Roles.Admin))
                return Forbid();
            
            var claims = roles
                .Select(x => new Claim(ClaimTypes.Role, x.Role))
                .Concat(new[]
                {
                    new Claim(ClaimTypes.Name, user.Name ?? ""),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("sub", user.Id.ToString())
                })
                .ToList();
            
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                IsPersistent = true,
                IssuedUtc = DateTimeOffset.Now
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            
            return Ok();
        }

        [HttpGet("logout")]
        public async Task<IActionResult> AdminLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
        
        [HttpGet("check")]
        [Authorize(Policy = "Admin")]
        public Task PulseAdmin()
        {
            return Task.CompletedTask;
        }
    }
}