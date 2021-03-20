using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto;
using QuartierLatin.Backend.Dto.AdminPageModuleDto;
using QuartierLatin.Backend.Dto.PageModuleDto;
using QuartierLatin.Backend.Models;

namespace QuartierLatin.Backend.Application.Interfaces
{
    public interface IPageAppService
    {
        Task<RouteDto<PageModuleDto>> GetPageByUrlAsync(string url);

        Task<RouteDto<AdminPageModuleDto>> GetPageByUrlAdminAsync(string url);

        Task<int> CreatePageAsync(string url, int languageId, string title,
            JObject pageData);

        Task CreateOrUpdatePageLanguageAsync(int pageRootId, string url, int languageId, string title,
            JObject pageData);

        Task<(Dictionary<int, string>, (int totalResults, List<(int id, List<Page> pages)> results))>
            GetPageListBySearch(int page, string search, int pageSize);
    }
}
