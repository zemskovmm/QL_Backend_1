using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Models.Repositories
{
    public interface IPageRepository
    {
        Task<int> CreatePageAsync(int languageId, string url, string title, JObject pageData);

        Task CreateOrUpdatePageLanguageAsync(int pageRootId, int languageId, string url, string title,
            JObject pageData);
        public Task<IList<Page>> GetPagesByPageUrlAsync(string url);

        public Task<IList<Page>> GetPagesByPageRootIdAsync(int pageRootId);

        public Task<Page> GetPagesByPageRootIdAndLanguageIdAsync(int pageRootId, int languageId);

        public Task<int> RemovePageAsync(int pageId);

        public Task EditPageAsync(Page page);

        Task<(int totalResults, List<(int id, List<Page> pages)> results)> GetPageRootsWithPagesAsync(string search,
            int skip, int take);
    }
}
