using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.CourseCatalog.SchoolCatalog;

namespace QuartierLatin.Backend.Controllers.CourseCatalog.School
{
    [Authorize(Roles = "Admin")]
    [Route("/api/admin/school/gallery")]
    public class AdminSchoolGalleryController : Controller
    {
        private readonly ISchoolGalleryAppService _schoolGalleryAppService;
        public AdminSchoolGalleryController(ISchoolGalleryAppService schoolGalleryAppService)
        {
            _schoolGalleryAppService = schoolGalleryAppService;
        }

        [HttpPost("{schoolId}/{imageId}")]
        public async Task<IActionResult> CreateGalleryItemToSchool(int schoolId, int imageId)
        {
            await _schoolGalleryAppService.CreateGalleryItemToSchoolAsync(schoolId, imageId);
            return Ok(new object());
        }

        [HttpDelete("{schoolId}/{imageId}")]
        public async Task<IActionResult> DeleteGalleryItemToSchool(int schoolId, int imageId)
        {
            await _schoolGalleryAppService.DeleteGalleryItemToSchoolAsync(schoolId, imageId);
            return Ok(new object());
        }
    }
}
