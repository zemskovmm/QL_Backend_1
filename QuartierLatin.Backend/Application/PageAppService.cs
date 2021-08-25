using System;
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
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Backend.Utils;
using QuartierLatin.Backend.Models.Repositories.AppStateRepository;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;

namespace QuartierLatin.Backend.Application
{
    public class PageAppService : IPageAppService
    {
        private readonly IPageRepository _pageRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IAppStateEntryRepository _appStateEntryRepository;
        private readonly ICommonTraitRepository _commonTraitRepository;

        public PageAppService(IPageRepository pageRepository, ILanguageRepository languageRepository,
            IAppStateEntryRepository appStateEntryRepository, ICommonTraitRepository commonTraitRepository)
        {
            _pageRepository = pageRepository;
            _languageRepository = languageRepository;
            _appStateEntryRepository = appStateEntryRepository;
            _commonTraitRepository = commonTraitRepository;
        }

        private async Task<List<Page>> Convert(Dictionary<string, (string url, string title,
            JObject pageData, DateTime? date, int? previewImageId, int? smallPreviewImageId, int? widePreviewImageId, JObject? metadata)> languages)
        {
            var langs = (await _languageRepository.GetLanguageListAsync()).ToDictionary(x => x.LanguageShortName,
                x => x.Id);
            return languages.Select(x => new Page
            {
                Url = x.Value.url,
                Title = x.Value.title,
                PageData = x.Value.pageData.ToString(),
                LanguageId = langs[x.Key],
                PreviewImageId = x.Value.previewImageId,
                SmallPreviewImageId = x.Value.smallPreviewImageId,
                WidePreviewImageId = x.Value.widePreviewImageId,
                Metadata = x.Value.metadata?.ToString(),
                Date = x.Value.date
            }).ToList();
        }

        public async Task<int> CreatePageAsync(Dictionary<string, (string url, string title,
            JObject pageData, DateTime? date, int? previewImageId, int? smallPreviewImageId, int? widePreviewImageId, JObject? metadata)> languages, PageType pageType) =>
            await _pageRepository.CreatePages(await Convert(languages), pageType);
        
        public async Task UpdatePage(int id, Dictionary<string, (string url, string title, JObject pageData, DateTime? date, int? previewImageId, int? smallPreviewImageId, int? widePreviewImageId, JObject? metadata)> languages, PageType pageType) 
            => await _pageRepository.UpdatePages(id, await Convert(languages), pageType);

        public Task<IList<Page>> GetPageLanguages(int id) => _pageRepository.GetPagesByPageRootIdAsync(id);

        public async Task<(Dictionary<int, string> lang, (int totalResults, List<(int id, List<Page> pages)> results) result)> GetPageListBySearch(int page, string search, int pageSize, PageType? pageType)
        {
            var langs = await _languageRepository.GetLanguageIdWithShortNameAsync();
            var results = await _pageRepository.GetPageRootsWithPagesAsync(search, pageSize * page, pageSize, pageType);

            return (lang: langs, result: results);
        }

        public async Task<AdminRouteDto<AdminPageModuleDto>> GetPageByUrlAdminAsync(string url)
        {
            var clearUrl = RewriteRouteRules.ReWriteRequests(url);

            var pages = (List<Page>)await _pageRepository.GetPagesByPageUrlAsync(clearUrl);

            if (pages.Count is 0)
                return null;

            var languageIds = await _languageRepository.GetLanguageIdWithShortNameAsync();

            var urls = pages.ToDictionary(page => languageIds[page.LanguageId], page => page.Url);

            var titles = urls.ToDictionary(url => url.Key, url => pages.FirstOrDefault(page => page.Url == url.Value).Title);

            var blocks = pages.ToDictionary(page => languageIds[page.LanguageId], page => JObject.Parse(page.PageData));

            var dates = pages.Select(page => page.Date).ToList();

            var metadata = pages.ToDictionary(page => languageIds[page.LanguageId], page => page.Metadata is null ? null : JObject.Parse(page.Metadata));

            var pageRoot = await _pageRepository.GetPageRootByIdAsync(pages.First().PageRootId);

            var adminPageDto = new AdminPageDto(titles, blocks, dates, metadata);

            var adminPageModuleDto = new AdminPageModuleDto(adminPageDto, pageRoot.Id, pageRoot.PageType);

            var response = new AdminRouteDto<AdminPageModuleDto>(null, urls, adminPageModuleDto, "page", titles);

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

            var pageRoot = await _pageRepository.GetPageRootByIdAsync(pageMain.PageRootId);

            var pageDto = new Dto.PageModuleDto.PageDto(pageMain.Title, JObject.Parse(pageMain.PageData), pageMain.Date, pageRoot.PageType,
                pageMain.PreviewImageId, pageMain.SmallPreviewImageId, pageMain.WidePreviewImageId, null, null, pageMain.Metadata is null ? null : JObject.Parse(pageMain.Metadata));

            var pageModuleDto = new PageModuleDto(pageDto);

            var response = new RouteDto<PageModuleDto>(null, urls, pageModuleDto, "page", pageMain.Title);

            return response;
        }

        public async Task<PageRoot> GetPageRootByIdAsync(int id)
        {
            return await _pageRepository.GetPageRootByIdAsync(id);
        }

        public async Task<(int totalItems, List<(PageRoot pageRoot, Page page)>)> GetPagesByFilter(string lang, PageType entityType, Dictionary<string, List<int>> commonTraits, int pageNumber, int pageSize)
        {
            var commonTraitsIds = commonTraits
                .Select(x => x.Value).ToList();

            var langId = await _languageRepository.GetLanguageIdByShortNameAsync(lang);

            return await _pageRepository.GetPagesByFilter(commonTraitsIds, langId, pageSize * pageNumber, pageSize, entityType);
        }

        public async Task<Dictionary<int, List<CommonTrait>>> GetCommonTraitListByPageIds(IEnumerable<int> ids)
        {
            return await _commonTraitRepository.GetCommonTraitListByPageIds(ids);
        }
    }
}
