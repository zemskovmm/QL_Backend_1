﻿using System.Collections.Generic;
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

        public async Task<int> CreatePageAsync(string url, int languageId, string title, JObject pageData)
        {
            return await _pageRepository.CreatePageAsync(languageId, url, title, pageData);
        }

        public async Task CreateOrUpdatePageLanguageAsync(int pageRootId, string url, int languageId, string title, JObject pageData)
        {
            await _pageRepository.CreateOrUpdatePageLanguageAsync(pageRootId, languageId, url, title, pageData);
        }

        public async Task<(Dictionary<int, string>, (int totalResults, List<(int id, List<Page> pages)> results))> GetPageListBySearch(int page, string search, int pageSize)
        {
            var langs = (await _languageRepository.GetLanguageListAsync()).ToDictionary(x => x.Id,
                x => x.LanguageShortName);
            var results = await _pageRepository.GetPageRootsWithPagesAsync(search, pageSize * page, pageSize);

            return (langs, results);
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

            var adminPageModuleDto = new AdminPageModuleDto(adminPageDto, pages.First().PageRootId);

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

            var pageDto = new Dto.PageModuleDto.PageDto(pageMain.Title, JObject.Parse(pageMain.PageData));

            var pageModuleDto = new PageModuleDto(pageDto);

            var response = new RouteDto<PageModuleDto>(urls, pageModuleDto, "page");

            return response;
        }
    }
}
