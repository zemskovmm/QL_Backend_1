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
        Task<RouteDto<PageModuleDto>> GetPagesByRootIdAsync(string url);
        Task<IList<Page>> GetPagesByRootIdAsync(int id);

        Task<RouteDto<AdminPageModuleDto>> GetPageByUrlAdminAsync(string url);

        Task<int> CreatePageAsync(Dictionary<string, (string url, string title, JObject pageData)> languages);

        Task<IList<Page>> GetPageLanguages(int id);

        Task UpdatePage(int id, Dictionary<string, (string url, string title, JObject pageData)> languages);

        Task<(Dictionary<int, string>, (int totalResults, List<(int id, List<Page> pages)> results))>
            GetPageListBySearch(int page, string search, int pageSize);
    }
}
