using QuartierLatin.Backend.Models.CatalogModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.Interfaces.Catalog
{
    public interface IUniversityAppService
    {
        Task<List<(University university, Dictionary<int, UniversityLanguage> universityLanguage)>> GetUniversityListAsync();
        Task<(University university, Dictionary<int, UniversityLanguage> universityLanguage)> GetUniversityByIdAsync(int id);
        Task UpdateUniversityByIdAsync(int id, int? foundationYear);
        Task UpdateUniversityLanguageByIdAsync(int id, string description, int languageId, string name, string url);
        Task<int> CreateUniversityAsync(int? universityFoundationYear);
        Task CreateUniversityLanguageListAsync(List<UniversityLanguage> universityLanguage);
        Task<(University university, Dictionary<int, UniversityLanguage> universityLanguage)> GetUniversityByUrlWithLanguage(int languageId, string url);
        Task<List<Specialty>> GetSpecialtiesUniversityByUniversityId(int universityId);
    }
}
