using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.Interfaces.PortalServices;
using QuartierLatin.Backend.Dto.PortalApplicationDto;
using QuartierLatin.Backend.Models.Portal;

namespace QuartierLatin.Backend.Controllers
{
    [Route("/api/personal")]
    public class PersonalController : Controller
    {
        private readonly IPersonalAppService _personalAppService;
        public PersonalController(IPersonalAppService personalAppService)
        {
            _personalAppService = personalAppService;
        }

        [HttpPost("applications")]
        public async Task<IActionResult> CreateApplication([FromBody] CreatePortalApplicationDto createApplication)
        {
            var id = await _personalAppService.CreateApplicationAsync(createApplication.Type, createApplication.EntityId,
                createApplication.CommonApplicationInfo, createApplication.EntityTypeSpecificApplicationInfo);

            return Ok(new { id = id });
        }

        [HttpPost("applications/{id}")]
        public async Task<IActionResult> UpdateApplication(int id, [FromBody]UpdatePortalApplicationDto updatePortalApplication)
        {
            await _personalAppService.UpdateApplicationAsync(id, updatePortalApplication.Type,
                updatePortalApplication.EntityId, updatePortalApplication.CommonApplicationInfo,
                updatePortalApplication.EntityTypeSpecificApplicationInfo);

            return Ok();
        }

        [HttpGet("applications/{id}")]
        public async Task<IActionResult> GetApplication(int id)
        {
            var application = await _personalAppService.GetApplicationAsync(id);

            var response = new PortalApplicationDto
            {
                Id = application.Id,
                Type = application.Type,
                EntityId = application.EntityId,
                Status = application.Status,
                CommonApplicationInfo = application.CommonTypeSpecificApplicationInfo is null ? null : JObject.Parse(application.CommonTypeSpecificApplicationInfo),
                EntityTypeSpecificApplicationInfo = application.EntityTypeSpecificApplicationInfo is null ? null : JObject.Parse(application.EntityTypeSpecificApplicationInfo)
            };

            return Ok(response);
        }

        [HttpGet("applications")]
        public async Task<IActionResult> GetApplicationCatalog()
        {


            return Ok();
        }
    }
}
