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
            
            if (!roles.Any(x => x.Role == Roles.Admin || x.Role == Roles.Manager))
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

        [Authorize(Roles = "Admin")]
        [HttpPost("manager/register")]
        public async Task<IActionResult> ManagerRegister([FromBody] AdminRegisterFormDto model)
        {
            AdminProfileDto user;

            try
            {
                await _user.RegisterAdmin(model, Roles.Manager);
                user = _user.Login(model.Email, model.Password);
            }
            catch (Exception e)
            {
                return Forbid();
            }

            var roles = await _roles.GetUserRolesByIds(new List<int> { user.Id });

            if (!roles.Any(x => x.Role == Roles.Admin || x.Role == Roles.Manager))
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
            return Ok();
        }
        
        [HttpGet("check")]
        [Authorize(Roles = "Admin")]
        public Task PulseAdmin()
        {
            return Task.CompletedTask;
        }

        [HttpGet("roles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RoleList()
        {
            return Ok(Roles.ValidRolesList);
        }
    }
}