using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CourseCatalogModels.SchoolModels;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CourseCatalogRepository.SchoolRepository
{
    public interface ISchoolCatalogRepository
    {
        Task<List<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)>> GetSchoolListAsync();
        Task<int> CreateSchoolAsync(int? foundationYear, int? imageId, List<SchoolLanguages> schoolLanguage);
        Task<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)> GetSchoolByIdAsync(int id);
        Task UpdateSchoolByIdAsync(int id, int? foundationYear, int? imageId);
        Task CreateOrUpdateSchoolLanguageByIdAsync(int schoolId, string htmlDescription, int languageId, string name, string url, JObject? metadata);
        Task<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)> GetSchoolByUrlWithLanguageAsync(int languageId, string url);
        Task<Dictionary<int, (int? schoolImageId, string schoolName, string schoolUrl)>> GetSchoolImageIdAndNameAndUrlByIdsAsync(IEnumerable<int> schoolIds, string lang);
    }
}
