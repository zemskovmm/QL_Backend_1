using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Models;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Dto;
using QuartierLatin.Backend.Dto.AdminPageModuleDto;
using QuartierLatin.Backend.Dto.PageModuleDto;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services
{
    public interface IPageAppService
    {
        Task<RouteDto<PageModuleDto>> GetPagesByRootIdAsync(string lang, string url);
        Task<IList<Page>> GetPagesByRootIdAsync(int id);

        Task<AdminRouteDto<AdminPageModuleDto>> GetPageByUrlAdminAsync(string url);

        Task<int> CreatePageAsync(Dictionary<string, (string url, string title, JObject pageData, DateTime? date, int? previewImageId, int? smallPreviewImageId, int? widePreviewImageId, JObject? metadata)> languages, PageType pageType);

        Task<IList<Page>> GetPageLanguages(int id);

        Task UpdatePage(int id, Dictionary<string, (string url, string title, JObject pageData, DateTime? date, int? previewImageId, int? smallPreviewImageId, int? widePreviewImageId, JObject? metadata)> languages, PageType pageType);

        Task<(Dictionary<int, string> lang, (int totalResults, List<(int id, List<Page> pages)> results) result)>
            GetPageListBySearch(int page, string search, int pageSize, PageType? pageType);

        Task<PageRoot> GetPageRootByIdAsync(int id);
        Task<(int totalItems, List<(PageRoot pageRoot, Page page)>)> GetPagesByFilter(string lang, PageType entityType, Dictionary<string, List<int>> commonTraits, int pageNumber, int pageSize);
        Task<Dictionary<int, List<CommonTrait>>> GetCommonTraitListByPageIds(IEnumerable<int> ids);
    }
}
