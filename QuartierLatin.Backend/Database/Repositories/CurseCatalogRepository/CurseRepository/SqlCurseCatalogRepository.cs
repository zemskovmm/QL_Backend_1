using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            throw new NotImplementedException();
        }

        public async Task<int> CreateCurseAsync(int schoolId)
        {
            throw new NotImplementedException();
        }

        public async Task CreateCurseLanguageListAsync(List<CurseLanguage> curseLanguage)
        {
            throw new NotImplementedException();
        }

        public async Task<(Curse curse, Dictionary<int, CurseLanguage> schoolLanguage)> GetCurseByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCurseByIdAsync(int id, int schoolId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCurseLanguageByIdAsync(int id, string htmlDescription, int languageId, string name, string url)
        {
            throw new NotImplementedException();
        }
    }
}
