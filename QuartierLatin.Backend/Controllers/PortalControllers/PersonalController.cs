using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.Interfaces.PortalServices;
using QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto.CatalogSearchResponseDto;
using QuartierLatin.Backend.Dto.PortalApplicationDto;
using QuartierLatin.Backend.Models.Constants;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Backend.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Controllers.PortalControllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationPortal.AuthenticationScheme)]
    [Route("/api/personal/applications")]
    public class PersonalController : Controller
    {
        private readonly IPortalPersonalAppService _personalAppService;
        public PersonalController(IPortalPersonalAppService personalAppService)
        {
            _personalAppService = personalAppService;
        }

        [HttpPost(),
         ProducesResponseType(200)]
        public async Task<IActionResult> CreateApplication([FromBody] PortalApplicationWithoutIdDto createApplication)
        {
            var userClaims = User.Identities.FirstOrDefault(identity =>
                identity.AuthenticationType == CookieAuthenticationPortal.AuthenticationScheme).Claims;

            var userId = Convert.ToInt32(userClaims.FirstOrDefault(claim => claim.Type == "sub").Value);

            var id = await _personalAppService.CreateApplicationAsync(createApplication.Type, createApplication.EntityId,
                createApplication.CommonApplicationInfo, createApplication.EntityTypeSpecificApplicationInfo, userId);

            return Ok(new { id = id });
        }

        [HttpPost("{id}"),
         ProducesResponseType(200),
         ProducesResponseType(403)]
        public async Task<IActionResult> UpdateApplication(int id, [FromBody]PortalApplicationWithoutIdDto updatePortalApplication)
        {
            var response = await _personalAppService.UpdateApplicationAsync(id, updatePortalApplication.Type,
                updatePortalApplication.EntityId, updatePortalApplication.CommonApplicationInfo,
                updatePortalApplication.EntityTypeSpecificApplicationInfo);

            if (response is false)
                return Forbid();

            return Ok();
        }

        [HttpGet("{id}"),
         ProducesResponseType(typeof(PortalApplicationDto), 200),
         ProducesResponseType( 404)]
        public async Task<IActionResult> GetApplication(int id)
        {
            var application = await _personalAppService.GetApplicationAsync(id);

            if (application is null)
                return NotFound();

            var response = new PortalApplicationDto
            {
                Id = application.Id,
                Type = application.Type,
                EntityId = application.EntityId,
                Status = application.Status,
                CommonApplicationInfo = JObject.Parse(application.CommonTypeSpecificApplicationInfo),
                EntityTypeSpecificApplicationInfo = JObject.Parse(application.EntityTypeSpecificApplicationInfo)
            };

            return Ok(response);
        }

        [HttpGet(),
         ProducesResponseType(typeof(CatalogSearchResponseDtoList<PortalApplicationDto>), 200)]
        public async Task<IActionResult> GetApplicationCatalog([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] ApplicationType? type, [FromQuery] ApplicationStatus? status)
        {
            var portalApplicationList = await _personalAppService.GetApplicationCatalogAsync(type, status, page, pageSize);

            var portalDtos = portalApplicationList.portalApplications.Select(application => new PortalApplicationDto
            {
                Id = application.Id,
                Type = application.Type,
                EntityId = application.EntityId,
                Status = application.Status,
                CommonApplicationInfo = JObject.Parse(application.CommonTypeSpecificApplicationInfo),
                EntityTypeSpecificApplicationInfo = JObject.Parse(application.EntityTypeSpecificApplicationInfo)
            }).ToList();

            var response = new CatalogSearchResponseDtoList<PortalApplicationDto>
            {
                Items = portalDtos,
                TotalItems = portalApplicationList.totalItems,
                TotalPages = FilterHelper.PageCount(portalApplicationList.totalItems, pageSize)
            };

            return Ok(response);
        }
    }
}
