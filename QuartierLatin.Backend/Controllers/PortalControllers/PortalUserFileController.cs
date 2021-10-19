using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Constants;
using QuartierLatin.Backend.Dto.Media;

namespace QuartierLatin.Backend.Controllers.PortalControllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationPortal.AuthenticationScheme)]
    [Route("/api/personal/media")]
    public class PortalUserFileController : Controller
    {
        private readonly IFileAppService _fileAppService;
        public PortalUserFileController(IFileAppService fileAppService)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedia(int id)
        {
            await _fileAppService.DeleteFileAsync(id);

            return Ok(new object());
        }
    }
}
