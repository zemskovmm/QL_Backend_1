using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Dto.Media;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/api/admin/media")]
    public class AdminFileController : Controller
    {
        private readonly IFileAppService _fileAppService;
        public AdminFileController(IFileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        
        [HttpPost()]
        public async Task<IActionResult> CreateMedia([FromForm] CreateMediaDto createMediaDto)
        {
            var response = await _fileAppService.UploadFileAsync(createMediaDto.UploadedFile.OpenReadStream(),
                createMediaDto.UploadedFile.FileName, createMediaDto.UploadedFile.ContentType, null, null, createMediaDto.StorageFolderId);

            return Ok(new { id = response });
        }
    }
}
