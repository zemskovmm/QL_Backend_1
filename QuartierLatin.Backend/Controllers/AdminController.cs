using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Dto.AdminPageModuleDto;
using QuartierLatin.Backend.Models.Repositories;

namespace QuartierLatin.Backend.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPageAppService _pageAppService;
        private readonly IDataBlockAppService _dataBlockAppService;
        private readonly ILanguageRepository _languageRepository;
        private readonly IPageRepository _pageRepository;

        public AdminController(IPageAppService pageAppService, IDataBlockAppService dataBlockAppService, ILanguageRepository languageRepository,
            IPageRepository pageRepository)
        {
            _pageAppService = pageAppService;
            _dataBlockAppService = dataBlockAppService;
            _languageRepository = languageRepository;
            _pageRepository = pageRepository;
        }

        static int PageCount(int resultCount, int pageSize) =>
            resultCount / pageSize + (resultCount % pageSize == 0 ? 0 : 1);

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost("/api/admin/pages/")]
        public async Task<IActionResult> CreatePage([FromBody] CreateNewPageDto createPageDto)
        {
            var languageId =  await _languageRepository.GetLanguageIdByShortNameAsync(createPageDto.Language);

            var response = await _pageAppService.CreatePageAsync(createPageDto.Url, languageId, createPageDto.Title, createPageDto.PageData);

            return Ok(new {id = response});
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("/api/admin/pages/{id}/{lang}")]
        public async Task<IActionResult> CreateOrUpdatePageLanguage([FromBody] PageDto pageDto, int id, string lang)
        {
            var languageId = await _languageRepository.GetLanguageIdByShortNameAsync(lang);

            await _pageAppService.CreateOrUpdatePageLanguageAsync(id, pageDto.Url, languageId, pageDto.Title, pageDto.PageData);

            return Ok(new object());
        }
    }
}
