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

            provider.TryGetContentType(response.Value.Item3, out var contentType);

            return File(response.Value.Item1, contentType, response.Value.Item3);
        }

        [AllowAnonymous]
        [HttpGet("/media/scaled/{id}")]
        public async Task<IActionResult> GetCompressedMedia(long id, [FromQuery]int dimension)
        {
            await using var stream = new MemoryStream();

            var responseFromService = await _fileAppService.GetFileAsync(id, dimension);

            if (responseFromService is null)
            {
                responseFromService = await _fileAppService.GetFileAsync(id);

                var imageScaler = new ImageScaler(dimension);

                imageScaler.Scale(responseFromService.Value.Item1, stream);

                var fileContent = stream.ToArray();

                await using var fileStream = new MemoryStream(fileContent);

                await _fileAppService.UploadFileAsync(fileStream, responseFromService.Value.Item3, responseFromService.Value.Item2, dimension, id);

                var provider = new FileExtensionContentTypeProvider();

                provider.TryGetContentType(responseFromService.Value.Item3, out var contentType);

                return File(fileContent, contentType, responseFromService.Value.Item3);
            }
            else
            {
                var provider = new FileExtensionContentTypeProvider();

                provider.TryGetContentType(responseFromService.Value.Item3, out var contentType);

                await responseFromService.Value.Item1.CopyToAsync(stream);

                return File(stream.ToArray(), contentType, responseFromService.Value.Item3);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/media/")]
        public async Task<IActionResult> CreateMedia([FromForm] CreateMediaDto createMediaDto)
        {
            var response = await _fileAppService.UploadFileAsync(createMediaDto.UploadedFile.OpenReadStream(), createMediaDto.UploadedFile.FileName, createMediaDto.FileType);

            return Ok(new {id = response});
        }
    }
}
