using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Dto.Media;
using System.Threading.Tasks;
using QuartierLatin.Backend.Utils;

namespace QuartierLatin.Backend.Controllers
{
    public class FileController : Controller
    {
        private readonly IFileAppService _fileAppService;
        public FileController(IFileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        [AllowAnonymous]
        [HttpGet("/media/{id}")]
        public async Task<IActionResult> GetMedia(long id)
        {
            var response = await _fileAppService.GetFileAsync(id);

            var provider = new FileExtensionContentTypeProvider();

            provider.TryGetContentType(response.Item3, out var contentType);


            return File(response.Item1, contentType, response.Item3);
        }

        [AllowAnonymous]
        [HttpGet("/media/scaled/{id}")]
        public async Task<IActionResult> GetCompressedMedia(long id, [FromQuery]int dimension)
        {
            await using var stream = new MemoryStream();

            var responseFromService = await _fileAppService.GetFileAsync(id);

            var imageScaler = new ImageScaler(dimension);

            imageScaler.Scale(responseFromService.Item1, stream);

            var provider = new FileExtensionContentTypeProvider();

            provider.TryGetContentType(responseFromService.Item3, out var contentType);

            return File(stream.ToArray(), contentType, responseFromService.Item3);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/media/")]
        public async Task<IActionResult> CreateMedia([FromForm] CreateMediaDto createMediaDto)
        {
            var response = await _fileAppService.UploadFileAsync(createMediaDto.UploadedFile, createMediaDto.FileType);

            return Ok(new {id = response});
        }
    }
}
