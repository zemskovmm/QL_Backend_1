using System.Collections.Generic;
using System.Threading.Tasks;
using LinqToDB;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.Repositories;
using System.Linq;

namespace QuartierLatin.Backend.Database.Repositories
{
    public class SqlLanguageRepository : ILanguageRepository
    {
        private readonly AppDbContextManager _db;

        public SqlLanguageRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public int CreateLanguage(string languageName, string languageShortName)
        {
            return _db.Exec(db => db.InsertWithInt32Identity(new Language
            {
                LanguageName = languageName,
                LanguageShortName = languageShortName
            }));
        }

        public async Task EditLanguageAsync(Language language)
        {
            await _db.ExecAsync(db => db
                .UpdateAsync(language));
        }

        public async Task<IList<Language>> GetLanguageListAsync()
        {
            return await _db.ExecAsync(db => db.Languages.ToListAsync());
        }

        public async Task<string> GetLanguageShortNameAsync(int languageId)
        {
            return await _db.ExecAsync(db => db.Languages.Where(language => language.Id == languageId).Select(language => language.LanguageShortName).FirstAsync());
        }

        public async Task<int> GetLanguageIdByShortNameAsync(string languageShortName)
        {
            return await _db.ExecAsync(db => db.Languages.Where(language => language.LanguageShortName == languageShortName).Select(language => language.Id).FirstAsync());
        }

        public async Task<int> RemoveLanguageAsync(int languageId)
        {
            return await _db.ExecAsync(db => db.Languages.Where(language => language.Id == languageId).DeleteAsync());
        }
    }
}
