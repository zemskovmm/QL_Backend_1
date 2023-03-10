using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Models;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories
{
    public interface IPageRepository
    {
        public Task<IList<Page>> GetPagesByPageUrlAsync(string url);

        public Task<IList<Page>> GetPagesByPageRootIdAsync(int pageRootId);

        public Task<Page> GetPagesByPageRootIdAndLanguageIdAsync(int pageRootId, int languageId);

        public Task<int> CreatePages(IList<Page> pagesWithoutRoot, PageType pageType);
        public Task UpdatePages(int rootId, IList<Page> pages, PageType pageType);

        public Task EditPageAsync(Page page);

        Task<(int totalResults, List<(int id, List<Page> pages)> results)> GetPageRootsWithPagesAsync(string search,
            int skip, int take, PageType? pageType);

        Task<PageRoot> GetPageRootByIdAsync(int id);
        Task<(int totalItems, List<(PageRoot pageRoot, Page page)>)> GetPagesByFilter(List<List<int>> commonTraitsIds, int langId, int skip, int take, PageType entityType);
    }
}
