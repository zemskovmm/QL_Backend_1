using LinqToDB;
using LinqToDB.Data;
using QuartierLatin.Backend.Models.CurseCatalogModels.CursesModels;
using QuartierLatin.Backend.Models.Repositories.CurseCatalogRepository.CurseRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<(Curse curse, Dictionary<int, CurseLanguage> curseLanguage)> GetCurseByUrlWithLanguageAsync(int languageId, string url)
        {
            return await _db.ExecAsync(async db =>
            {
                var curseId =
                    db.CurseLanguages.FirstOrDefault(curse =>
                        curse.Url == url && curse.LanguageId == languageId).CurseId;

                var curse = await db.Curses.FirstOrDefaultAsync(curse => curse.Id == curseId);

                var curseLanguage = await db.CurseLanguages.Where(curseLang => curseLang.CurseId == curse.Id)
                    .ToDictionaryAsync(curseLang => curseLang.LanguageId, curseLang => curseLang);

                return (curse: curse, curseLanguage: curseLanguage);
            });
        }

        public async Task<(int totalItems, List<(Curse curse, CurseLanguage curseLanguage)>)> GetCursePageByFilter(List<List<int>> commonTraitsIds, int langId, int skip, int take)
        {
            return await _db.ExecAsync(async db =>
            {
                var curses = db.Curses.AsQueryable();

                if (commonTraitsIds.Any())
                {
                    var curseWithTraits = db.CommonTraitToCurses.AsQueryable();
                    var schoolWithTrait = db.CommonTraitToSchools.AsQueryable();

                    foreach (var commonTraitGroup in commonTraitsIds)
                    {
                        if (commonTraitGroup.Count != 0)
                            schoolWithTrait =
                                schoolWithTrait.Where(t => commonTraitGroup.Contains(t.CommonTraitId));
                    }

                    foreach (var commonTraitGroup in commonTraitsIds)
                    {
                        if (commonTraitGroup.Count != 0)
                            curseWithTraits =
                                curseWithTraits.Where(t => commonTraitGroup.Contains(t.CommonTraitId));
                    }

                    curses = curses.Where(curse =>
                        curseWithTraits.Select(x => x.CurseId).Contains(curse.Id) &&
                        schoolWithTrait.Select(x => x.SchoolId).Contains(curse.SchoolId));
                }

                var curseWithLanguages = from curse in curses
                                                join lang in db.CurseLanguages.Where(l => l.LanguageId == langId)
                                                    on curse.Id equals lang.CurseId
                                                select new
                                                {
                                                    curse,
                                                    lang
                                                };

                var totalCount = await curseWithLanguages.CountAsync();

                return (totalCount,
                    (await curseWithLanguages.OrderBy(x => x.curse.Id).Skip(skip).Take(take).ToListAsync())
                    .Select(x => (curse: x.curse, curseLanguage: x.lang)).ToList());
            });
        }
    }
}
