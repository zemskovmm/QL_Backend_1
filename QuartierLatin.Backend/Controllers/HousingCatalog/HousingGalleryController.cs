using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces.HousingServices;

namespace QuartierLatin.Backend.Controllers.HousingCatalog
{
    [AllowAnonymous]
    [Route("/api/housing/gallery")]
    public class HousingGalleryController : Controller
    {
        private readonly IHousingGalleryAppService _housingGalleryAppService;

        public HousingGalleryController(IHousingGalleryAppService housingGalleryAppService)
        {
            _housingGalleryAppService = housingGalleryAppService;
        }

        [HttpGet("{housingId}")]
        public async Task<IActionResult> GetGalleryToUniversity(int housingId)
        {
            var response = await _housingGalleryAppService.GetGalleryToHousingAsync(housingId);
            return Ok(response);
        }
    }
}
