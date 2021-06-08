using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Models.CurseCatalogModels.CursesModels;

namespace QuartierLatin.Backend.Models.Repositories.CurseCatalogRepository.CurseRepository
{
    public interface ICurseCatalogRepository
    {
        Task<List<(Curse curse, Dictionary<int, CurseLanguage> curseLanguage)>> GetCurseListAsync();
        Task<int> CreateCurseAsync(int schoolId);
        Task CreateCurseLanguageListAsync(List<CurseLanguage> curseLanguage);
        Task<(Curse curse, Dictionary<int, CurseLanguage> schoolLanguage)> GetCurseByIdAsync(int id);
        Task UpdateCurseByIdAsync(int id, int schoolId);
        Task UpdateCurseLanguageByIdAsync(int id, string htmlDescription, int languageId, string name, string url);
    }
}
