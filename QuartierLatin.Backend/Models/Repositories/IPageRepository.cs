using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Models.Repositories
{
    public interface IPageRepository
    {
        public Task<int> CreatePageAsync(string url, int languageId, string title, JObject pageData, int pageRootId);

        public Task<IList<Page>> GetPagesByPageUrlAsync(string url);

        public Task<int> RemovePageAsync(int pageId);

        public Task EditPageAsync(Page page);
    }
}
