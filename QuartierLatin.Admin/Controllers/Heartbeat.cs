using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuartierLatin.Admin.Controllers
{
    [Route("/api/heartbeat")]
    [Authorize]
    public class Heartbeat : Controller
    {
        [HttpGet("admin")]
        [Authorize(Policy = "Admin")]
        public Task PulseAdmin()
        {
            return Task.CompletedTask;
        }
    }
}