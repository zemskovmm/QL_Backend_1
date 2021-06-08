using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.Interfaces.CurseCatalog.CurseCatalog;
using QuartierLatin.Backend.Models.CurseCatalogModels.CursesModels;
using QuartierLatin.Backend.Models.Repositories.CurseCatalogRepository.CurseRepository;

namespace QuartierLatin.Backend.Application.CurseCatalog.CurseCatalog
{
    public class CurseAppService : ICurseAppService
    {
        private readonly ICurseCatalogRepository _curseCatalogRepository;

        public CurseAppService(ICurseCatalogRepository curseCatalogRepository)
        {
            _curseCatalogRepository = curseCatalogRepository;
        }

        public async Task<List<(Curse curse, Dictionary<int, CurseLanguage> curseLanguage)>> GetCurseListAsync()
        {
            return await _curseCatalogRepository.GetCurseListAsync();
        }

        public async Task<int> CreateCurseAsync(int schoolId)
        {
            return await _curseCatalogRepository.CreateCurseAsync(schoolId);
        }

        public async Task CreateCurseLanguageListAsync(List<CurseLanguage> curseLanguage)
        {
            await _curseCatalogRepository.CreateCurseLanguageListAsync(curseLanguage);
        }

        public async Task<(Curse curse, Dictionary<int, CurseLanguage> schoolLanguage)> GetCurseByIdAsync(int id)
        {
            return await _curseCatalogRepository.GetCurseByIdAsync(id);
        }

        public async Task UpdateCurseByIdAsync(int id, int schoolId)
        {
            await _curseCatalogRepository.UpdateCurseByIdAsync(id, schoolId);
        }

        public async Task UpdateCurseLanguageByIdAsync(int id, string htmlDescription, int languageId, string name, string url)
        {
            await _curseCatalogRepository.UpdateCurseLanguageByIdAsync(id, htmlDescription, languageId, name, url);
        }
    }
}
