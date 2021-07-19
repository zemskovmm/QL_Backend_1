using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Dto.ImageStandardSizeDto;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.Interfaces.ImageStandardSizeService;

namespace QuartierLatin.Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/api/admin")]
    public class AdminImageStandardSizeController : Controller
    {
        private readonly IImageStandardSizeAppService _imageStandardSizeAppService;

        public AdminImageStandardSizeController(IImageStandardSizeAppService imageStandardSizeAppService)
        {
            _imageStandardSizeAppService = imageStandardSizeAppService;
        }

        [HttpGet("image-standard-size/{id}")]
        public async Task<IActionResult> GetImageStandardSizeById(int id)
        {
            var imageStandardSizeEntity = await _imageStandardSizeAppService.GetImageStandardSizeByIdAsync(id);

            var response = new AdminImageStandardSizeDto
            {
                Width = imageStandardSizeEntity.Width,
                Height = imageStandardSizeEntity.Height
            };

            return Ok(response);
        }

        [HttpGet("image-standard-size")]
        public async Task<IActionResult> GetImageStandardSizeList()
        {
            var imageStandardSizeEntityList = await _imageStandardSizeAppService.GetImageStandardSizeListAsync();

            var response = imageStandardSizeEntityList.Select(imageStandard => new AdminListImageStandardSizeDto
            {
                Id = imageStandard.Id,
                Height = imageStandard.Height,
                Width = imageStandard.Width
            }).ToList();

            return Ok(response);
        }

        [HttpPost("image-standard-size")]
        public async Task<IActionResult> CreateImageStandardSize([FromBody] AdminImageStandardSizeDto imageStandardSize)
        {
            var response = await _imageStandardSizeAppService.CreateImageStandardSizeAsync(imageStandardSize.Height,
                imageStandardSize.Width);

            return Ok(new { id = response });
        }

        [HttpPut("image-standard-size/{id}")]
        public async Task<IActionResult> UpdateImageStandardSizeById([FromBody] AdminImageStandardSizeDto imageStandardSize, int id)
        {
            await _imageStandardSizeAppService.UpdateImageStandardSizeByIdAsync(id, imageStandardSize.Height,
                imageStandardSize.Width);

            return Ok(new object());
        }

        [HttpDelete("image-standard-size/{id}")]
        public async Task<IActionResult> DeleteImageStandardSizeById(int id)
        {
            await _imageStandardSizeAppService.DeleteImageStandardSizeByIdAsync(id);

            return Ok(new object());
        }
    }
}
