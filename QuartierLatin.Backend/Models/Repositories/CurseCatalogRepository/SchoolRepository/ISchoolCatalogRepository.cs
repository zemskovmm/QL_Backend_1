using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Models.CurseCatalogModels.SchoolModels;

namespace QuartierLatin.Backend.Models.Repositories.CurseCatalogRepository.SchoolRepository
{
    public interface ISchoolCatalogRepository
    {
        Task<List<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)>> GetSchoolListAsync();
        Task<int> CreateSchoolAsync(int? foundationYear);
        Task CreateSchoolLanguageListAsync(List<SchoolLanguages> schoolLanguage);
        Task<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)> GetSchoolByIdAsync(int id);
        Task UpdateSchoolByIdAsync(int id, int? foundationYear);
        Task CreateOrUpdateSchoolLanguageByIdAsync(int schoolId, string htmlDescription, int languageId, string name, string url);
    }
}
