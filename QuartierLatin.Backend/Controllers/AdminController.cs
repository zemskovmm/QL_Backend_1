using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Dto.AdminPageModuleDto;
using QuartierLatin.Backend.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/api/admin/pages")]
    public class AdminController : Controller
    {
        private readonly IPageAppService _pageAppService;

        public AdminController(IPageAppService pageAppService)
        {
            _pageAppService = pageAppService;
        }

        [HttpGet(), ProducesResponseType(typeof(PageListDto), 200)]
        public async Task<IActionResult> GetPageList([FromQuery]int page, [FromQuery]string search)
        {
            const int pageSize = 10;
            var result = await _pageAppService.GetPageListBySearch(page, search, pageSize);

            return Ok(new PageListDto
            {
                TotalPages = FilterHelper.PageCount(result.result.totalResults, pageSize),
                Results = result.result.results.Select(x => new PageListItemDto
                {
                    Id = x.id,
                    Titles = x.pages.ToDictionary(x => result.lang[x.LanguageId], x => x.Title),
                    Urls = x.pages.ToDictionary(x => result.lang[x.LanguageId], x => x.Url)
                }).ToList()
            });
        }

        
        [HttpPost()]
        public async Task<IActionResult> CreatePage([FromBody] PageDto createPageDto)
        {
            var id = await _pageAppService.CreatePageAsync(createPageDto.Languages.ToDictionary(x => x.Key,
                x => (x.Value.Url, x.Value.Title, x.Value.PageData)));
            return Ok(new {id = id});
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePage(int id, [FromBody] PageDto createPageDto)
        {
            await _pageAppService.UpdatePage(id, createPageDto.Languages.ToDictionary(x => x.Key,
                x => (x.Value.Url, x.Value.Title, x.Value.PageData)));
            return Ok(new {id = id});
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPage(int id, [FromBody] PageDto createPageDto)
        {
            await _pageAppService.UpdatePage(id, createPageDto.Languages.ToDictionary(x => x.Key,
                x => (x.Value.Url, x.Value.Title, x.Value.PageData)));
            return Ok(new {id = id});
        }
        
    }
}
