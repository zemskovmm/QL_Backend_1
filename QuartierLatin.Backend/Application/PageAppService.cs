using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Dto;
using QuartierLatin.Backend.Dto.PageModuleDto;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.Repositories;
using System.Linq;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto.AdminPageModuleDto;
using QuartierLatin.Backend.Utils;

namespace QuartierLatin.Backend.Application
{
    public class PageAppService : IPageAppService
    {
        private readonly IPageRepository _pageRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IDataBlockRepository _dataBlockRepository;

        public PageAppService(IPageRepository pageRepository, ILanguageRepository languageRepository, IDataBlockRepository dataBlockRepository)
        {
            _pageRepository = pageRepository;
            _languageRepository = languageRepository;
            _dataBlockRepository = dataBlockRepository;
        }

        public async Task<int> CreatePageAsync(CreatePageDto createPageDto)
        {
            return await _pageRepository.CreatePageAsync(createPageDto.Url, createPageDto.LanguageId, createPageDto.Title, createPageDto.PageData, createPageDto.PageRootId);
        }

        public async Task<RouteDto<AdminPageModuleDto>> GetPageByUrlAdminAsync(string url)
        {
            var clearUrl = RewriteRouteRules.ReWriteRequests(url);

            var pages = (List<Page>)await _pageRepository.GetPagesByPageUrlAsync(clearUrl);

            if (pages.Count is 0)
                return null;

            var urls = pages.ToDictionary(page => _languageRepository.GetLanguageShortNameAsync(page.LanguageId)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult(), page => page.Url);

            var titles = urls.ToDictionary(url => url.Key, url => pages.FirstOrDefault(page => page.Url == url.Value).Title);

            var blocks = pages.ToDictionary(page => _languageRepository.GetLanguageShortNameAsync(page.LanguageId)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult(), page => JObject.Parse(page.PageData));

            var adminPageDto = new AdminPageDto(titles, blocks);

            var adminPageModuleDto = new AdminPageModuleDto(adminPageDto);

            var response = new RouteDto<AdminPageModuleDto>(urls, adminPageModuleDto, "page");

            return response;
        }

        public async Task<RouteDto<PageModuleDto>> GetPageByUrlAsync(string url)
        {
            var clearUrl = RewriteRouteRules.ReWriteRequests(url);

            var pages = (List<Page>)await _pageRepository.GetPagesByPageUrlAsync(clearUrl);

            if (pages.Count is 0)
                return null;

            var pageMain = pages.Find(page => page.Url == clearUrl);

            var urls = pages.ToDictionary(page => _languageRepository.GetLanguageShortNameAsync(page.LanguageId)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult(), page => page.Url);

            var dataBlocks = (List<DataBlock>)await _dataBlockRepository.GetDataBlockListForPageAndLanguageAsync(pageMain.LanguageId, pageMain.PageRootId);

            var dataBlockDtos = dataBlocks.Select(block => new PageBlockDto(block.Type, JObject.Parse(block.BlockData)));

            var pageDto = new PageDto(pageMain.Title, dataBlockDtos);

            var pageModuleDto = new PageModuleDto(pageDto);

            var response = new RouteDto<PageModuleDto>(urls, pageModuleDto, "page");

            return response;
        }
    }
}
