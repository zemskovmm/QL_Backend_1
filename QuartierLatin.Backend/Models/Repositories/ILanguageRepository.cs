using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Models.Repositories
{
    public interface ILanguageRepository
    {
        public int CreateLanguage(string languageName, string languageShortName);

        public Task<IList<Language>> GetLanguageListAsync();

        public Task<int> RemoveLanguageAsync(int languageId);

        public Task EditLanguageAsync(Language language);

        public Task<string> GetLanguageShortNameAsync(int languageId);
        public Task<int> GetLanguageIdByShortNameAsync(string languageShortName);
        Task<string> GetLanguageNameById(int languageId);
    }
}
