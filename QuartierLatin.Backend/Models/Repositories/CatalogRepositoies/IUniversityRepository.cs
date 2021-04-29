using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Models.CatalogModels;

namespace QuartierLatin.Backend.Models.Repositories.CatalogRepositoies
{
    public interface IUniversityRepository
    {
        Task CreateOrUpdateUniversityLanguageAsync(int universityId, int languageId, string name, string description, string url);
        Task<List<int>> GetUniversityIdListAsync();
        Task<Dictionary<int, UniversityLanguage>> GetUniversityLanguageByUniversityIdAsync(int universityId);
        Task<University> GetUniversityByIdAsync(int id);
        Task CreateUniversityLanguageListAsync(List<UniversityLanguage> universityLanguage);
        Task<int> CreateUniversityAsync(int? foundationYear, string website);
        Task UpdateUniversityAsync(int id, int? foundationYear, string website);
        Task<int> GetUniversityIdByUrl(string url);
        Task<Specialty> GetSpecialtyById(int specialtyId);
        Task<int> GetUniversityIdByUrlAndLanguage(int languageId, string url);
        Task<(int totalPages, List<(University university, UniversityLanguage universityLanguage, int cost)> universities)> GetUniversityPageByFilter(List<List<int>> commonTraitGroups, List<int> specialtyCategoriesId, List<int> priceIds, int languageId, int skip, int take);
    }
}
