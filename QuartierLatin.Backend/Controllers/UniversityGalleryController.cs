using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.Catalog;

namespace QuartierLatin.Backend.Controllers
{
    [AllowAnonymous]
    [Route("/api/university/gallery")]
    public class UniversityGalleryController : Controller
    {
        private readonly IUniversityGalleryAppService _universityGalleryAppService;
        public UniversityGalleryController(IUniversityGalleryAppService universityGalleryAppService)
        {
            _universityGalleryAppService = universityGalleryAppService;
        }

        [HttpGet("{universityId}")]
        public async Task<IActionResult> GetGalleryToUniversity(int universityId)
        {
            var response = await _universityGalleryAppService.GetGalleryToUniversityAsync(universityId);
            return Ok(response);
        }
    }
}
