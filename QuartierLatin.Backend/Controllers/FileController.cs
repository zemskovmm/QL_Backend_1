using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services;

namespace QuartierLatin.Backend.Controllers
{
    [AllowAnonymous]
    [Route("/api/media")]
    public class FileController : Controller
    {
        private readonly IFileAppService _fileAppService;
        public FileController(IFileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedia(int id)
        {
            var response = await _fileAppService.GetFileAsync(id);

            var provider = new FileExtensionContentTypeProvider();

            provider.TryGetContentType(response.Value.Item3, out var contentType);

            return File(response.Value.Item1, contentType, response.Value.Item3);
        }

        [HttpGet("scaled/{id}")]
        public async Task<IActionResult> GetCompressedMedia(int id, [FromQuery]int? dimension, [FromQuery] int? standardSizeId)
        {
            var responseStandardSize = await _fileAppService.GetCompressedFileAsync(id, dimension, standardSizeId);

            var providerStandardSize = new FileExtensionContentTypeProvider();

            providerStandardSize.TryGetContentType(responseStandardSize.Value.Item3, out var contentTypeStandardSize);

            return File(responseStandardSize.Value.Item1, contentTypeStandardSize, responseStandardSize.Value.Item3);
        }
    }
}
