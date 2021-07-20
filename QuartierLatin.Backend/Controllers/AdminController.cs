using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Dto.AdminPageModuleDto;
using QuartierLatin.Backend.Utils;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Backend.Models.Repositories;

namespace QuartierLatin.Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/api/admin/pages")]
    public class AdminController : Controller
    {
        private readonly IPageAppService _pageAppService;
        private readonly ILanguageRepository _languageRepository;

        public AdminController(IPageAppService pageAppService, ILanguageRepository languageRepository)
        {
            _pageAppService = pageAppService;
            _languageRepository = languageRepository;
        }

        [HttpGet(), ProducesResponseType(typeof(PageListDto), 200)]
        public async Task<IActionResult> GetPageList([FromQuery]int page, [FromQuery]string search, [FromQuery]PageType pageType)
        {
            const int pageSize = 10;
            var result = await _pageAppService.GetPageListBySearch(page, search, pageSize, pageType);

            return Ok(new PageListDto
            {
                TotalPages = FilterHelper.PageCount(result.result.totalResults, pageSize),
                Results = result.result.results.Select(x => new PageListItemDto
                {
                    Id = x.id,
                    Titles = x.pages.ToDictionary(x => result.lang[x.LanguageId], x => x.Title),
                    Urls = x.pages.ToDictionary(x => result.lang[x.LanguageId], x => x.Url),
                    PreviewImages = x.pages.ToDictionary(x => result.lang[x.LanguageId], x => x.PreviewImageId)
                }).ToList()
            });
        }

        
        [HttpPost()]
        public async Task<IActionResult> CreatePage([FromBody] PageDto createPageDto)
        {
            var id = await _pageAppService.CreatePageAsync(createPageDto.Languages.ToDictionary(x => x.Key,
                x => (x.Value.Url, x.Value.Title, x.Value.PageData, x.Value.Date, x.Value.PreviewImageId, x.Value.Metadata)), createPageDto.PageType);
            return Ok(new {id = id});
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePage(int id, [FromBody] PageDto createPageDto)
        {
            await _pageAppService.UpdatePage(id, createPageDto.Languages.ToDictionary(x => x.Key,
                x => (x.Value.Url, x.Value.Title, x.Value.PageData, x.Value.Date, x.Value.PreviewImageId, x.Value.Metadata)), createPageDto.PageType);
            return Ok(new {id = id});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPage(int id)
        {
            var page = await _pageAppService.GetPageRootByIdAsync(id);
            var allLangs = (await _languageRepository.GetLanguageListAsync())
                .ToDictionary(x => x.Id, x => x.LanguageShortName);
            var pageLangs = (await _pageAppService.GetPageLanguages(id))
                .ToDictionary(x => allLangs[x.LanguageId]);

            return Ok(new PageDto
            {
                PageType = page.PageType,
                Languages = pageLangs.ToDictionary(x => x.Key,
                    x => new PageLanguageDto()
                    {
                        Url = x.Value.Url,
                        Title = x.Value.Title,
                        PageData = JObject.Parse(x.Value.PageData),
                        PreviewImageId = x.Value.PreviewImageId,
                        Metadata = x.Value.Metadata is null ? null : JObject.Parse(x.Value.Metadata),
                        Date = x.Value.Date
                    })
            });
        }
    }
}
