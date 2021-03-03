using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuartierLatin.Admin.Controllers
{
    [Route("[Controller]")]
    [Authorize]
    public class Heartbeat : Controller
    {
        [HttpGet("pulseAdmin")]
        [Authorize(Policy = "Admin")]
        public Task PulseAdmin()
        {
            return Task.CompletedTask;
        }
    }
}