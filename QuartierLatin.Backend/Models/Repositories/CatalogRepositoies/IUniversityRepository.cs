using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.CatalogModels;

namespace QuartierLatin.Backend.Models.Repositories.CatalogRepositoies
{
    public interface IUniversityRepository
    {
        Task<int> CreateUniversityAsync(int? foundationYear, int? logoId, int? bannerId, List<UniversityLanguage> universityLanguage);
        Task<List<int>> GetUniversityIdListAsync();
        Task<Dictionary<int, UniversityLanguage>> GetUniversityLanguageByUniversityIdAsync(int universityId);
        Task<University> GetUniversityByIdAsync(int id);
        Task UpdateUniversityAsync(int id, int? foundationYear, int? logoId, int? bannerId);
        Task<int> GetUniversityIdByUrl(string url);
        Task<List<Specialty>> GetSpecialtiesUniversityByUniversityIdList(int universityId);
        Task<Specialty> GetSpecialtyById(int specialtyId);
        Task<int> GetUniversityIdByUrlAndLanguage(int languageId, string url);
        Task<(int totalItems, List<(University university, UniversityLanguage universityLanguage, int cost)>)> GetUniversityPageByFilter(
            List<List<int>> commonTraitGroups, List<int> specialtyCategoriesId, List<int> degreeIds, List<int> priceIds,
            int languageId, int skip, int take);

        Task UpdateUniversityLanguageByIdAsync(int id, string description, int languageId, string name,
            string url, JObject? metadata);
    }
}
