using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Dto.PortalDto;

namespace QuartierLatin.Backend.Controllers.PortalControllers
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
