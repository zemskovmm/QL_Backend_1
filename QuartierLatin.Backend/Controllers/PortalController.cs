using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Dto.PortalDto;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Controllers
{
    [Route("/api/portal")]
    public class PortalController : Controller
    {
        public PortalController()
        {
            
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody]PortalRegisterDto portalRegister)
        {
            

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] PortalLoginModel portalLogin)
        {


            return Ok();
        }
    }
}
