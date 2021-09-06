using QuartierLatin.Backend.Models.CourseCatalogModels.SchoolModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Application.Interfaces.courseCatalog.SchoolCatalog
{
    public interface ISchoolAppService
    {
        Task<List<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)>> GetSchoolListAsync();
        Task<int> CreateSchoolAsync(int? foundationYear, int? imageId, List<SchoolLanguages> schoolLanguage);
        Task<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)> GetSchoolByIdAsync(int id);
        Task UpdateSchoolByIdAsync(int id, int? schoolDtoFoundationYear, int? imageId);
        Task UpdateSchoolLanguageByIdAsync(int id, string htmlDescription, int languageId, string name, string url, JObject? metadata);
        Task<Dictionary<int, (int? schoolImageId, string schoolName)>> GetSchoolImageIdAndNameByIdsAsync(IEnumerable<int> schoolIds, string lang);
    }
}
