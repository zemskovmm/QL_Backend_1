using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.Catalog
{
    public interface IUniversityAppService
    {
        Task<List<(University university, Dictionary<int, UniversityLanguage> universityLanguage)>> GetUniversityListAsync();
        Task<(University university, Dictionary<int, UniversityLanguage> universityLanguage)> GetUniversityByIdAsync(int id);
        Task UpdateUniversityByIdAsync(int id, int? foundationYear, int? logoId, int? bannerId);
        Task UpdateUniversityLanguageByIdAsync(int id, string description, int languageId, string name, string url, JObject? metadata);
        Task<int> CreateUniversityAsync(int? foundationYear, int? logoId, int? bannerId,
            List<UniversityLanguage> universityLanguage);
        Task<(University university, Dictionary<int, UniversityLanguage> universityLanguage)> GetUniversityByUrlWithLanguage(int languageId, string url);
        Task<List<Specialty>> GetSpecialtiesUniversityByUniversityId(int universityId);
    }
}
