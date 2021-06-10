using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LinqToDB;
using System.Linq;
using LinqToDB.Data;
using QuartierLatin.Backend.Models.CurseCatalogModels.CursesModels;
using QuartierLatin.Backend.Models.Repositories.CurseCatalogRepository.CurseRepository;

namespace QuartierLatin.Backend.Database.Repositories.CurseCatalogRepository.CurseRepository
{
    public class SqlCurseCatalogRepository : ICurseCatalogRepository
    {
        private readonly AppDbContextManager _db;

        public SqlCurseCatalogRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<List<(Curse curse, Dictionary<int, CurseLanguage> curseLanguage)>> GetCurseListAsync()
        {
            return await _db.ExecAsync(async db =>
            {
                var curses = await db.Curses.Select(curse => GetCurseByIdAsync(curse.Id)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult()).ToListAsync();

                return curses;
            });
        }

        public async Task<int> CreateCurseAsync(int curseId)
        {
            return await _db.ExecAsync(db => db.InsertWithInt32IdentityAsync(new Curse
            {
                SchoolId = curseId,
            }));
        }

        public async Task CreateCurseLanguageListAsync(List<CurseLanguage> curseLanguage)
        {
            await _db.ExecAsync(db => db.BulkCopyAsync(curseLanguage));
        }

        public async Task<(Curse curse, Dictionary<int, CurseLanguage> curseLanguage)> GetCurseByIdAsync(int id)
        {
            return await _db.ExecAsync(async db =>
            {
                var curse = await db.Curses.FirstOrDefaultAsync(curse => curse.Id == id);

                var curseLanguage = await db.CurseLanguages.Where(curseLang => curseLang.CurseId == curse.Id)
                    .ToDictionaryAsync(curseLang => curseLang.LanguageId, curseLang => curseLang);

                return (curse: curse, curseLanguage: curseLanguage);
            });
        }

        public async Task UpdateCurseByIdAsync(int id, int schoolId)
        {
            await _db.ExecAsync(db => db.UpdateAsync(new Curse
            {
                Id = id,
                SchoolId = schoolId
            }));
        }

        public async Task CreateOrUpdateCurseLanguageByIdAsync(int id, string htmlDescription, int languageId, string name, string url)
        {
            await _db.ExecAsync(db => db.InsertOrReplaceAsync(new CurseLanguage
            {
                CurseId = id,
                LanguageId = languageId,
                Description = htmlDescription,
                Name = name,
                Url = url
            }));
        }
    }
}
