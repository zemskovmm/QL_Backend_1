using QuartierLatin.Backend.Models.CatalogModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Dto.UniversityDto.GetUniversityListDto;

namespace QuartierLatin.Backend.Application.Interfaces.Catalog
{
    public interface IUniversityAppService
    {
        Task<List<(University, Dictionary<int, UniversityLanguage>)>> GetUniversityListAsync();
        Task<(University, Dictionary<int, UniversityLanguage>)> GetUniversityByIdAsync(int id);
        Task UpdateUniversityByIdAsync(int id, int? foundationYear, string website);
        Task UpdateUniversityLanguageByIdAsync(int id, string description, int languageId, string name, string url);
        Task<int> CreateUniversityAsync(int? universityFoundationYear, string universityWebsite);
        Task CreateUniversityLanguageListAsync(List<UniversityLanguage> universityLanguage);
    }
}
