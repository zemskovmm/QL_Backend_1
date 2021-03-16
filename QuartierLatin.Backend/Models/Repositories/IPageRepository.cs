using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Models.Repositories
{
    public interface IPageRepository
    {
        public int CreatePage(string url, int languageId, string title, int pageRootId);

        public Task<IList<Page>> GetPagesByPageUrlAsync(string url);

        public Task<int> RemovePageAsync(int pageId);

        public Task EditPageAsync(Page page);
    }
}
