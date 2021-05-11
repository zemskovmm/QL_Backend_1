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

        public PageAppService(IPageRepository pageRepository, ILanguageRepository languageRepository)
        {
            _pageRepository = pageRepository;
            _languageRepository = languageRepository;
        }

        private async Task<List<Page>> Convert(Dictionary<string, (string url, string title,
            JObject pageData)> languages)
        {
            var langs = (await _languageRepository.GetLanguageListAsync()).ToDictionary(x => x.LanguageShortName,
                x => x.Id);
            return languages.Select(x => new Page
            {
                Url = x.Value.url,
                Title = x.Value.title,
                PageData = x.Value.pageData.ToString(),
                LanguageId = langs[x.Key]
            }).ToList();
        }

        public async Task<int> CreatePageAsync(Dictionary<string, (string url, string title,
            JObject pageData)> languages) =>
            await _pageRepository.CreatePages(await Convert(languages));
        
        public async Task UpdatePage(int id, Dictionary<string, (string url, string title, JObject pageData)> languages) 
            => await _pageRepository.UpdatePages(id, await Convert(languages));

        public Task<IList<Page>> GetPageLanguages(int id) => _pageRepository.GetPagesByPageRootIdAsync(id);

        public async Task<(Dictionary<int, string> lang, (int totalResults, List<(int id, List<Page> pages)> results) result)> GetPageListBySearch(int page, string search, int pageSize)
        {
            var langs = await _languageRepository.GetLanguageIdWithShortNameAsync();
            var results = await _pageRepository.GetPageRootsWithPagesAsync(search, pageSize * page, pageSize);

            return (lang: langs, result: results);
        }

        public async Task<RouteDto<AdminPageModuleDto>> GetPageByUrlAdminAsync(string url)
        {
            var clearUrl = RewriteRouteRules.ReWriteRequests(url);

            var pages = (List<Page>)await _pageRepository.GetPagesByPageUrlAsync(clearUrl);

            if (pages.Count is 0)
                return null;

            var languageIds = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var urls = pages.ToDictionary(page => languageIds[page.LanguageId], page => page.Url);

            var titles = urls.ToDictionary(url => url.Key, url => pages.FirstOrDefault(page => page.Url == url.Value).Title);

            var blocks = pages.ToDictionary(page => languageIds[page.LanguageId], page => JObject.Parse(page.PageData));

            var adminPageDto = new AdminPageDto(titles, blocks);

            var adminPageModuleDto = new AdminPageModuleDto(adminPageDto, pages.First().PageRootId);

            var response = new RouteDto<AdminPageModuleDto>(null, urls, adminPageModuleDto, "page");

            return response;
        }

        public Task<IList<Page>> GetPagesByRootIdAsync(int id) => _pageRepository.GetPagesByPageRootIdAsync(id);
        
        public async Task<RouteDto<PageModuleDto>> GetPagesByRootIdAsync(string lang, string url)
        {
            var clearUrl = RewriteRouteRules.ReWriteRequests(url);

            var pages = (List<Page>)await _pageRepository.GetPagesByPageUrlAsync(clearUrl);

            if (pages.Count is 0)
                return null;

            var languageIds = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var langId = languageIds.FirstOrDefault(language => language.Value == lang).Key;

            var pageMain = pages.Find(page => page.LanguageId == langId);

            var urls = pages.ToDictionary(page => languageIds[page.LanguageId], page => page.Url);

            var pageDto = new Dto.PageModuleDto.PageDto(pageMain.Title, JObject.Parse(pageMain.PageData));

            var pageModuleDto = new PageModuleDto(pageDto);

            var response = new RouteDto<PageModuleDto>(null, urls, pageModuleDto, "page");

            return response;
        }
    }
}
