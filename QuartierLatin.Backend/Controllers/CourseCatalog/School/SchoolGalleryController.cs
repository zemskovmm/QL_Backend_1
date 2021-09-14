using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.CourseCatalog.SchoolCatalog;

namespace QuartierLatin.Backend.Controllers.CourseCatalog.School
{
    [AllowAnonymous]
    [Route("/api/school/gallery")]
    public class SchoolGalleryController : Controller
    {
        private readonly ISchoolGalleryAppService _schoolGalleryAppService;
        public SchoolGalleryController(ISchoolGalleryAppService schoolGalleryAppService)
        {
            _schoolGalleryAppService = schoolGalleryAppService;
        }

        [HttpGet("{schoolId}")]
        public async Task<IActionResult> GetGalleryToSchool(int schoolId)
        {
            var response = await _schoolGalleryAppService.GetGalleryToSchoolAsync(schoolId);
            return Ok(response);
        }
    }
}
