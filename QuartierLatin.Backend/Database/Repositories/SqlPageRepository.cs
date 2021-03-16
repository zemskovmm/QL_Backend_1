using System.Collections.Generic;
using System.Threading.Tasks;
using LinqToDB;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.Repositories;
using System.Linq;

namespace QuartierLatin.Backend.Database.Repositories
{
    public class SqlPageRepository : IPageRepository
    {
        private readonly AppDbContextManager _db;

        public SqlPageRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public int CreatePage(string url, int languageId, string title, int pageRootId = 0)
        {
            var page = new Page
            {
                Url = url,
                LanguageId = languageId,
                PageRootId = pageRootId,
                Title = title
            };

            var rootId = _db.Exec(db => db.InsertWithInt32Identity(page));

            if (pageRootId is 0)
            {
                page.PageRootId = rootId;
                page.Id = rootId;
            }

            _db.Exec(db => db.Update(page));
            return rootId;
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
            return await _db.ExecAsync(db => db.Pages.Where(page => page.Id == pageId).DeleteAsync());
        }
    }
}
