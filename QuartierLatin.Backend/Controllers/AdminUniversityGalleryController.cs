using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.Interfaces.Catalog;

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

        [HttpGet("{universityId}")]
        public async Task<IActionResult> GetGalleryToUniversity(int universityId)
        {
            var response = await _universityGalleryAppService.GetGalleryToUniversityAsync(universityId);
            return Ok(response);
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
