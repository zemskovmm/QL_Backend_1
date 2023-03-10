using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.Catalog;

namespace QuartierLatin.Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/api/admin/university/gallery")]
    public class AdminUniversityGalleryController : Controller
    {
        private readonly IUniversityGalleryAppService _universityGalleryAppService;
        public AdminUniversityGalleryController(IUniversityGalleryAppService universityGalleryAppService)
        {
            _universityGalleryAppService = universityGalleryAppService;
        }

        [HttpPost("{universityId}/{imageId}")]
        public async Task<IActionResult> CreateGalleryItemToUniversity(int universityId, int imageId)
        {
            await _universityGalleryAppService.CreateGalleryItemToUniversityAsync(universityId, imageId);
            return Ok(new object());
        }

        [HttpDelete("{universityId}/{imageId}")]
        public async Task<IActionResult> DeleteGalleryItemToUniversity(int universityId, int imageId)
        {
            await _universityGalleryAppService.DeleteGalleryItemToUniversityAsync(universityId, imageId);
            return Ok(new object());
        }
    }
}
