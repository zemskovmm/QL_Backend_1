using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Models.CurseCatalogModels.SchoolModels;

namespace QuartierLatin.Backend.Application.Interfaces.CurseCatalog.SchoolCatalog
{
    public interface ISchoolAppService
    {
        Task<List<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)>> GetSchoolListAsync();
        Task<int> CreateSchoolAsync(int? schoolDtoFoundationYear);
        Task CreateSchoolLanguageListAsync(List<SchoolLanguages> schoolLanguage);
        Task<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)> GetSchoolByIdAsync(int id);
        Task UpdateSchoolByIdAsync(int id, int? schoolDtoFoundationYear);
        Task UpdateSchoolLanguageByIdAsync(int id, string htmlDescription, int languageId, string name, string url);
    }
}
