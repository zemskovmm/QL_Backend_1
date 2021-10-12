using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.HousingServices;

namespace QuartierLatin.Backend.Controllers.HousingCatalog
{
    [Authorize(Roles = "Admin")]
    [Route("/api/admin/housing/gallery")]
    public class AdminHousingGalleryController : Controller
    {
        private readonly IHousingGalleryAppService _housingGalleryAppService;

        public AdminHousingGalleryController(IHousingGalleryAppService housingGalleryAppService)
        {
            _housingGalleryAppService = housingGalleryAppService;
        }

        [HttpPost("{housingId}/{imageId}")]
        public async Task<IActionResult> CreateGalleryItemToUniversity(int housingId, int imageId)
        {
            await _housingGalleryAppService.CreateGalleryItemToHousingAsync(housingId, imageId);
            return Ok(new object());
        }

        [HttpDelete("{housingId}/{imageId}")]
        public async Task<IActionResult> DeleteGalleryItemToUniversity(int housingId, int imageId)
        {
            await _housingGalleryAppService.DeleteGalleryItemToHousingAsync(housingId, imageId);
            return Ok(new object());
        }
    }
}
