using LinqToDB;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Database.Repositories
{
    public class SqlPageRepository : IPageRepository
    {
        private readonly AppDbContextManager _db;

        public SqlPageRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public Task<int> CreatePageAsync(int languageId, string url, string title, JObject pageData)
        {
            return _db.ExecAsync(async db =>
            {
                await using var t = await db.BeginTransactionAsync();

                var pageRootId = await _db.ExecAsync(db => db.InsertWithInt32IdentityAsync(new PageRoot()));
                await CreateOrUpdatePageCore(db, pageRootId, languageId, url, title, pageData);
                await t.CommitAsync();
                return pageRootId;
            });
        }

        public Task CreateOrUpdatePageLanguageAsync(int pageRootId, int languageId, string url, string title,
            JObject pageData)
            => _db.ExecAsync(db => CreateOrUpdatePageCore(db, pageRootId, languageId, url, title, pageData));

        private static async Task CreateOrUpdatePageCore(AppDbContext db, int pageRootId, int languageId, string url, string title,
            JObject pageData)
        {
            await db.InsertOrReplaceAsync(new Page
            {
                Url = url,
                LanguageId = languageId,
                PageRootId = pageRootId,
                Title = title,
                PageData = pageData.ToString(Newtonsoft.Json.Formatting.None)
            });
        }

        public async Task EditPageAsync(Page page)
        {
            await _db.ExecAsync(db => db
                .UpdateAsync(page));
        }

        public Task<(int totalResults, List<(int id, List<Page> pages)> results)> GetPageRootsWithPagesAsync(string search, int skip, int take) =>
            _db.ExecAsync(async db =>
            {
                var q = db.PageRoots.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var ids = db.Pages.Where(p => p.Title.Contains(search) || p.Url.Contains(search))
                        .Select(p => p.PageRootId);
                    q = q.Where(x => ids.Contains(x.Id));
                }

                var count = await q.CountAsync();

                var idList = await q.Select(x => x.Id).ToListAsync();
                var pages = (await db.Pages.Where(x => idList.Contains(x.PageRootId)).ToListAsync()).GroupBy(x => x.PageRootId)
                    .ToDictionary(x => x.Key, x => x.ToList());

                return (count, idList.Select(id => (id, pages[id])).ToList());
            });

        public async Task<IList<Page>> GetPagesByPageUrlAsync(string url)
        {
            var pageRootId = await _db.ExecAsync(db => db.Pages.Where(page => page.Url == url).Select(page => page.PageRootId).FirstAsync());

            return await GetPagesByPageRootIdAsync(pageRootId);
        }

        public async Task<IList<Page>> GetPagesByPageRootIdAsync(int pageRootId)
        {
            return await _db.ExecAsync(db => db.Pages.Where(page => page.PageRootId == pageRootId).ToListAsync());
        }

        public async Task<Page> GetPagesByPageRootIdAndLanguageIdAsync(int pageRootId, int languageId)
        {
            return await _db.ExecAsync(db => db.Pages.FirstOrDefaultAsync(page => page.PageRootId == pageRootId && page.LanguageId == languageId));
        }

        public async Task<int> RemovePageAsync(int pageId)
        {
            return await _db.ExecAsync(db => db.Pages.Where(page => page.PageRootId == pageId).DeleteAsync());
        }
    }
}
