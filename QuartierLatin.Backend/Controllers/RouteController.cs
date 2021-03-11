using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuartierLatin.Backend.Controllers
{
    public class RouteController : Controller
    {
        [Authorize]
        [HttpGet("/api/route/{route}")]
        public async Task<IActionResult> GetPage(string route)
        {
            return View();
        }
    }
}
