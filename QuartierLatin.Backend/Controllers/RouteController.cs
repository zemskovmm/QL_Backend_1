using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces;

namespace QuartierLatin.Backend.Controllers
{
    public class RouteController : Controller
    {
        private readonly IRouteAppService _routeAppService;

        public RouteController(IRouteAppService routeAppService)
        {
            _routeAppService = routeAppService;
        }

        [Authorize]
        [HttpGet("/api/route/{route}")]
        public async Task<IActionResult> GetPage(string route)
        {
            var routeResponse = await _routeAppService.GetPageByUrlAsync(route);

            if (routeResponse is null)
                return BadRequest();

            return Ok(routeResponse);
        }

        [Authorize]
        [HttpGet("/admin/api/route/{route}")]
        public async Task<IActionResult> GetPageAdmin(string route)
        {
            var routeResponse = await _routeAppService.GetPageByUrlAdminAsync(route);

            if (routeResponse is null)
                return BadRequest();

            return Ok(routeResponse);
        }
    }
}
