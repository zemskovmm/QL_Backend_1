using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces.CourseCatalog.CourseCatalog;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Controllers.CourseCatalog.Course
{
    [AllowAnonymous]
    [Route("/api/course/gallery")]
    public class CourseGalleryController : Controller
    {
        private readonly ICourseGalleryAppService _courseGalleryAppService;
        public CourseGalleryController(ICourseGalleryAppService courseGalleryAppService)
        {
            _courseGalleryAppService = courseGalleryAppService;
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetGalleryToUniversity(int courseId)
        {
            var response = await _courseGalleryAppService.GetGalleryToCourseAsync(courseId);
            return Ok(response);
        }
    }
}
