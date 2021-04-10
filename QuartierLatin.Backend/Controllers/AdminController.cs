using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Dto.AdminPageModuleDto;
using QuartierLatin.Backend.Models.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IPageAppService _pageAppService;
        private readonly ILanguageRepository _languageRepository;
        private readonly IPageRepository _pageRepository;

        public AdminController(IPageAppService pageAppService,ILanguageRepository languageRepository,
            IPageRepository pageRepository)
        {
            _pageAppService = pageAppService;
            _languageRepository = languageRepository;
            _pageRepository = pageRepository;
        }

        static int PageCount(int resultCount, int pageSize) =>
            resultCount / pageSize + (resultCount % pageSize == 0 ? 0 : 1);
        
        [HttpGet("/api/admin/pages/"), ProducesResponseType(typeof(PageListDto), 200)]
        public async Task<IActionResult> GetPageList([FromQuery]int page, [FromQuery]string search)
        {
            const int pageSize = 10;
            var result = await _pageAppService.GetPageListBySearch(page, search, pageSize);

            return Ok(new PageListDto
            {
                TotalPages = PageCount(result.Item2.totalResults, pageSize),
                Results = result.Item2.results.Select(x => new PageListItemDto
                {
                    Id = x.id,
                    Titles = x.pages.ToDictionary(x => result.Item1[x.LanguageId], x => x.Title),
                    Urls = x.pages.ToDictionary(x => result.Item1[x.LanguageId], x => x.Url)
                }).ToList()
            });
        }

        
        [HttpPost("/api/admin/pages/")]
        public async Task<IActionResult> CreatePage([FromBody] PageDto createPageDto)
        {
            var id = await _pageAppService.CreatePageAsync(createPageDto.Languages.ToDictionary(x => x.Key,
                x => (x.Value.Url, x.Value.Title, x.Value.PageData)));
            return Ok(new {id = id});
        }
        
        [HttpPut("/api/admin/pages/{id}")]
        public async Task<IActionResult> UpdatePage(int id, [FromBody] PageDto createPageDto)
        {
            await _pageAppService.UpdatePage(id, createPageDto.Languages.ToDictionary(x => x.Key,
                x => (x.Value.Url, x.Value.Title, x.Value.PageData)));
            return Ok(new {id = id});
        }
        
        [HttpGet("/api/admin/pages/{id}")]
        public async Task<IActionResult> GetPage(int id, [FromBody] PageDto createPageDto)
        {
            _pageAppService.GetPagesByRootIdAsync(id);
            await _pageAppService.UpdatePage(id, createPageDto.Languages.ToDictionary(x => x.Key,
                x => (x.Value.Url, x.Value.Title, x.Value.PageData)));
            return Ok(new {id = id});
        }
        
    }
}
