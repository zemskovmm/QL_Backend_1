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

        public int CreatePage(CreatePageDto createPageDto)
        {
            return _pageRepository.CreatePage(createPageDto.Url, createPageDto.LanguageId, createPageDto.Title, createPageDto.PageRootId);
        }

        public async Task<RouteDto<AdminPageModuleDto>> GetPageByUrlAdminAsync(string url)
        {
            var pages = (List<Page>)await _pageRepository.GetPagesByPageUrlAsync(url);

            if (pages.Count is 0)
                return null;

            var pageMain = pages.Find(page => page.Url == url);

            var urls = pages.ToDictionary(page => _languageRepository.GetLanguageShortNameAsync(page.LanguageId)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult(), page => page.Url);

            var dataBlocks = (List<DataBlock>)await _dataBlockRepository.GetDataBlockListForPageAndLanguageAsync(pageMain.Id, pageMain.LanguageId);

            var titles = urls.ToDictionary(url => url.Key, url => pages.FirstOrDefault(page => page.Url == url.Value).Title);

            var blocksData = new List<AdminPageBlockDto>();

            foreach (var dataBlock in dataBlocks)
            {
                var blocks = await _dataBlockRepository.GetDataBlockListForPageByBlockRootIdAsync(dataBlock.BlockRootId);

                var dataBlocksWithLanguage = blocks.ToDictionary(block => _languageRepository.GetLanguageShortNameAsync(block.LanguageId)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult(), block => JObject.Parse(block.BlockData));

                var adminPageBlockDto = new AdminPageBlockDto(dataBlock.Type, dataBlocksWithLanguage);

                blocksData.Add(adminPageBlockDto);
            }

            var adminPageDto = new AdminPageDto(titles, blocksData);

            var adminPageModuleDto = new AdminPageModuleDto(adminPageDto);

            var response = new RouteDto<AdminPageModuleDto>(urls, adminPageModuleDto, "page");

            return response;
        }

        public async Task<RouteDto<PageModuleDto>> GetPageByUrlAsync(string url)
        {
            var pages = (List<Page>)await _pageRepository.GetPagesByPageUrlAsync(url);

            if (pages.Count is 0)
                return null;

            var pageMain = pages.Find(page => page.Url == url);

            var urls = pages.ToDictionary(page => _languageRepository.GetLanguageShortNameAsync(page.LanguageId)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult(), page => page.Url);

            var dataBlocks = (List<DataBlock>)await _dataBlockRepository.GetDataBlockListForPageAndLanguageAsync(pageMain.Id, pageMain.LanguageId);

            var dataBlockDtos = dataBlocks.Select(block => new PageBlockDto(block.Type, JObject.Parse(block.BlockData)));

            var pageDto = new PageDto(pageMain.Title, dataBlockDtos);

            var pageModuleDto = new PageModuleDto(pageDto);

            var response = new RouteDto<PageModuleDto>(urls, pageModuleDto, "page");

            return response;
        }
    }
}
