using QuartierLatin.Backend.Models.CourseCatalogModels.SchoolModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Models.Repositories.courseCatalogRepository.SchoolRepository
{
    public interface ISchoolCatalogRepository
    {
        Task<List<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)>> GetSchoolListAsync();
        Task<int> CreateSchoolAsync(int? foundationYear);
        Task CreateSchoolLanguageListAsync(List<SchoolLanguages> schoolLanguage);
        Task<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)> GetSchoolByIdAsync(int id);
        Task UpdateSchoolByIdAsync(int id, int? foundationYear);
        Task CreateOrUpdateSchoolLanguageByIdAsync(int schoolId, string htmlDescription, int languageId, string name, string url, JObject? metadata);
        Task<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)> GetSchoolByUrlWithLanguageAsync(int languageId, string url);
    }
}
