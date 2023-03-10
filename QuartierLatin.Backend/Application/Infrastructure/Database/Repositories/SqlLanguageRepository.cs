using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Models;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Repositories
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

        public async Task<string> GetLanguageNameById(int languageId)
        {
            return await _db.ExecAsync(db => db.Languages.Where(language => language.Id == languageId).Select(language => language.LanguageName).FirstAsync());
        }

        public async Task<Dictionary<int, string>> GetLanguageIdWithShortNameAsync()
        {
            return await _db.ExecAsync(db =>
                db.Languages.ToDictionaryAsync(language => language.Id, language => language.LanguageShortName));
        }

        public async Task<int> RemoveLanguageAsync(int languageId)
        {
            return await _db.ExecAsync(db => db.Languages.Where(language => language.Id == languageId).DeleteAsync());
        }
    }
}
