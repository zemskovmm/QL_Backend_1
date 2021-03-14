using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Dto;
using QuartierLatin.Backend.Dto.PageModuleDto;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.Repositories;
using System.Linq;
using Newtonsoft.Json.Linq;

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

        public async Task<RouteDto<PageModuleDto>> GetPageByUrl(string url)
        {
            var pages = (List<Page>)await _pageRepository.GetPagesByPageUrlAsync(url);

            if (pages.Count is 0)
                return null;

            var pageMain = pages.Find(page => page.Url == url);

            var urls = pages.ToDictionary(page => _languageRepository.GetLanguageShortNameAsync(page.LanguageId).ConfigureAwait(false).GetAwaiter().GetResult(), page => page.Url);

            var dataBlocks = (List<DataBlock>)await _dataBlockRepository.GetDataBlockListForPageAndLanguageAsync(pageMain.Id, pageMain.LanguageId);

            var dataBlockDtos = dataBlocks.Select(block => new PageBlockDto(block.Type, JObject.Parse(block.BlockData)));

            var pageDto = new PageDto(pageMain.Title, dataBlockDtos);

            var pageModuleDto = new PageModuleDto(pageDto);

            var response = new RouteDto<PageModuleDto>(urls, pageModuleDto, "page");

            return response;
        }
    }
}
