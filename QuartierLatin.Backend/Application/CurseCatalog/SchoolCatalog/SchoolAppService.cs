using QuartierLatin.Backend.Application.Interfaces.CurseCatalog.SchoolCatalog;
using QuartierLatin.Backend.Models.CurseCatalogModels.SchoolModels;
using QuartierLatin.Backend.Models.Repositories.CurseCatalogRepository.SchoolRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.CurseCatalog.SchoolCatalog
{
    public class SchoolAppService : ISchoolAppService
    {
        private readonly ISchoolCatalogRepository _schoolCatalogRepository;

        public SchoolAppService(ISchoolCatalogRepository schoolCatalogRepository)
        {
            _schoolCatalogRepository = schoolCatalogRepository;
        }

        public async Task<List<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)>> GetSchoolListAsync()
        {
            return await _schoolCatalogRepository.GetSchoolListAsync();
        }

        public async Task<int> CreateSchoolAsync(int? schoolDtoFoundationYear)
        {
            return await _schoolCatalogRepository.CreateSchoolAsync(schoolDtoFoundationYear);
        }

        public async Task CreateSchoolLanguageListAsync(List<SchoolLanguages> schoolLanguage)
        {
            await _schoolCatalogRepository.CreateSchoolLanguageListAsync(schoolLanguage);
        }

        public async Task<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)> GetSchoolByIdAsync(int id)
        {
            return await _schoolCatalogRepository.GetSchoolByIdAsync(id);
        }

        public async Task UpdateSchoolByIdAsync(int id, int? schoolDtoFoundationYear)
        {
            await _schoolCatalogRepository.UpdateSchoolByIdAsync(id, schoolDtoFoundationYear);
        }

        public async Task UpdateSchoolLanguageByIdAsync(int id, string valueHtmlDescription, int languageId, string valueName,
            string valueUrl)
        {
            await _schoolCatalogRepository.CreateOrUpdateSchoolLanguageByIdAsync(id, valueHtmlDescription, languageId,
                valueName, valueUrl);
        }
    }
}
