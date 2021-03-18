using System.Collections.Generic;
using System.Threading.Tasks;
using LinqToDB;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.Repositories;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Database.Repositories
{
    public class SqlPageRepository : IPageRepository
    {
        private readonly AppDbContextManager _db;

        public SqlPageRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<int> CreatePageAsync(string url, int languageId, string title, JObject pageData, int pageRootId = 0)
        {
            if (pageRootId is 0)
            {
                pageRootId = await _db.ExecAsync(db => db.InsertWithInt32IdentityAsync(new PageRoot()));
            }

            var page = new Page
            {
                Url = url,
                LanguageId = languageId,
                PageRootId = pageRootId,
                Title = title,
                PageData = pageData.ToString(Newtonsoft.Json.Formatting.None)
            };

            var pageId = await _db.ExecAsync(db => db.InsertAsync(page));

            return pageRootId;
        }

        public async Task EditPageAsync(Page page)
        {
            await _db.ExecAsync(db => db
                .UpdateAsync(page));
        }

        public async Task<IList<Page>> GetPagesByPageUrlAsync(string url)
        {
            var pageRootId = await _db.ExecAsync(db => db.Pages.Where(page => page.Url == url).Select(page => page.PageRootId).FirstAsync());

            return await _db.ExecAsync(db => db.Pages.Where(page => page.PageRootId == page.PageRootId).ToListAsync());
        }

        public async Task<int> RemovePageAsync(int pageId)
        {
            return await _db.ExecAsync(db => db.Pages.Where(page => page.PageRootId == pageId).DeleteAsync());
        }
    }
}
