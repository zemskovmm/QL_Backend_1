using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces.CourseCatalog.CourseCatalog;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Controllers.CourseCatalog.Course
{
    [Authorize(Roles = "Admin")]
    [Route("/api/admin/course/gallery")]
    public class AdminCourseGalleryController : Controller
    {
        private readonly ICourseGalleryAppService _courseGalleryAppService;
        public AdminCourseGalleryController(ICourseGalleryAppService courseGalleryAppService)
        {
            _courseGalleryAppService = courseGalleryAppService;
        }

        [HttpPost("{courseId}/{imageId}")]
        public async Task<IActionResult> CreateGalleryItemToCourse(int courseId, int imageId)
        {
            await _courseGalleryAppService.CreateGalleryItemToCourseAsync(courseId, imageId);
            return Ok(new object());
        }

        [HttpDelete("{courseId}/{imageId}")]
        public async Task<IActionResult> DeleteGalleryItemToUniversity(int courseId, int imageId)
        {
            await _courseGalleryAppService.DeleteGalleryItemToCourseAsync(courseId, imageId);
            return Ok(new object());
        }
    }
}
