using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.PortalServices;
using QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto.CatalogSearchResponseDto;
using QuartierLatin.Backend.Dto.PortalDto;
using QuartierLatin.Backend.Utils;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace QuartierLatin.Backend.Controllers.PortalControllers
{
    [Authorize]
    [Route("/api/admin/portal")]
    public class AdminPortalController : Controller
    {
        private readonly IPortalUserAppService _portalUserAppService;

        public AdminPortalController(IPortalUserAppService portalUserAppService)
        {
            _portalUserAppService = portalUserAppService;
        }

        [HttpGet("users"),
         ProducesResponseType(typeof(CatalogSearchResponseDtoList<PortalUserListAdminDto>), 200),
         ProducesResponseType(404)]
        public async Task<IActionResult> GetPortalUserAdminList([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? firstName, [FromQuery] string? lastName,
            [FromQuery] string? email, [FromQuery] string? phone)
        {
            var users = await _portalUserAppService.GetPortalUserAdminListAsync(firstName, lastName, email, phone, page, pageSize);

            var userDtos = users.users.Select(user => new PortalUserListAdminDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                Phone = user.Phone,
                RegistrationDate = user.RegistrationDate
            }).ToList();

            var response = new CatalogSearchResponseDtoList<PortalUserListAdminDto>
            {
                Items = userDtos,
                TotalItems = users.totalItems,
                TotalPages = FilterHelper.PageCount(users.totalItems, pageSize)
            };

            return Ok(response);
        }
    }
}