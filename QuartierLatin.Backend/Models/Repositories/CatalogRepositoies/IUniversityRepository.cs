using System.Threading.Tasks;
using QuartierLatin.Backend.Models.CatalogModels;

namespace QuartierLatin.Backend.Models.Repositories.CatalogRepositoies
{
    public interface IUniversityRepository
    {
        Task<int> CreateUniversityLanguageAsync(int languageId, string name, string description, string url, string website, int foundationYear);

        Task CreateOrUpdateUniversityLanguageAsync(int universityId, int languageId, string name, string description, string url);

        Task DeleteUniversityLanguageAsync(int universityId, int languageId);

        Task<(University, UniversityLanguage)> GetUniversityLanguageAsync(int universityId, int languageId);
    }
}
