using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Models.Repositories
{
    public interface IPageRepository
    {
        public Task<IList<Page>> GetPagesByPageUrlAsync(string url);

        public Task<IList<Page>> GetPagesByPageRootIdAsync(int pageRootId);

        public Task<Page> GetPagesByPageRootIdAndLanguageIdAsync(int pageRootId, int languageId);

        public Task<int> CreatePages(IList<Page> pagesWithoutRoot);
        public Task UpdatePages(int rootId, IList<Page> pages);

        public Task EditPageAsync(Page page);

        Task<(int totalResults, List<(int id, List<Page> pages)> results)> GetPageRootsWithPagesAsync(string search,
            int skip, int take);
    }
}
