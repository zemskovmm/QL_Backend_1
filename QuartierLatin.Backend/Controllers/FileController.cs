using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using QuartierLatin.Backend.Application.Interfaces;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Controllers
{
    [AllowAnonymous]
    [Route("/media")]
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
        public async Task<IActionResult> GetCompressedMedia(int id, [FromQuery]int dimension)
        {
            var response = await _fileAppService.GetCompressedFileAsync(id, dimension);

            var provider = new FileExtensionContentTypeProvider();

            provider.TryGetContentType(response.Value.Item3, out var contentType);

            return File(response.Value.Item1, contentType, response.Value.Item3);
        }
    }
}
