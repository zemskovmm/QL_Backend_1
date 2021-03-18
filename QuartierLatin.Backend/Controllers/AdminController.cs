using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Dto.AdminPageModuleDto;

namespace QuartierLatin.Backend.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPageAppService _pageAppService;
        private readonly IDataBlockAppService _dataBlockAppService;

        public AdminController(IPageAppService pageAppService, IDataBlockAppService dataBlockAppService)
        {
            _pageAppService = pageAppService;
            _dataBlockAppService = dataBlockAppService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/api/blocks/")]
        public async Task<IActionResult> CreateDataBlocks([FromBody] CreateDataBlockDto createDataBlocks)
        {
            var response = _dataBlockAppService.CreateDataBlockForPage(createDataBlocks);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/api/pages/")]
        public async Task<IActionResult> CreatePage([FromBody] CreatePageDto createPageDto)
        {
            var response = await _pageAppService.CreatePageAsync(createPageDto);

            return Ok(response);
        }
    }
}
