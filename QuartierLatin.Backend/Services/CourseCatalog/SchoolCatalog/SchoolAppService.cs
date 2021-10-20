using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.AppStateRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CourseCatalogRepository.SchoolRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.CourseCatalog.SchoolCatalog;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CourseCatalogModels.SchoolModels;

namespace QuartierLatin.Backend.Services.CourseCatalog.SchoolCatalog
{
    public class SchoolAppService : ISchoolAppService
    {
        private readonly ISchoolCatalogRepository _schoolCatalogRepository;
        private readonly IAppStateEntryRepository _appStateEntryRepository;

        public SchoolAppService(ISchoolCatalogRepository schoolCatalogRepository, IAppStateEntryRepository appStateEntryRepository)
        {
            _schoolCatalogRepository = schoolCatalogRepository;
            _appStateEntryRepository = appStateEntryRepository;
        }

        public async Task<List<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)>> GetSchoolListAsync()
        {
            return await _schoolCatalogRepository.GetSchoolListAsync();
        }

        public async Task<int> CreateSchoolAsync(int? foundationYear, int? imageId, List<SchoolLanguages> schoolLanguage)
        {
            var id = await _schoolCatalogRepository.CreateSchoolAsync(foundationYear, imageId, schoolLanguage);
            await _appStateEntryRepository.UpdateLastChangeTimeAsync();
            return id;
        }

        public async Task<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)> GetSchoolByIdAsync(int id)
        {
            return await _schoolCatalogRepository.GetSchoolByIdAsync(id);
        }

        public async Task UpdateSchoolByIdAsync(int id, int? schoolDtoFoundationYear, int? imageId)
        {
            await _schoolCatalogRepository.UpdateSchoolByIdAsync(id, schoolDtoFoundationYear, imageId);
            await _appStateEntryRepository.UpdateLastChangeTimeAsync();
        }

        public async Task UpdateSchoolLanguageByIdAsync(int id, string valueHtmlDescription, int languageId, string valueName,
            string valueUrl, JObject? metadata)
        {
            await _schoolCatalogRepository.CreateOrUpdateSchoolLanguageByIdAsync(id, valueHtmlDescription, languageId,
                valueName, valueUrl, metadata);
            await _appStateEntryRepository.UpdateLastChangeTimeAsync();
        }

        public async Task<Dictionary<int, (int? schoolImageId, string schoolName, string schoolUrl)>> GetSchoolImageIdAndNameAndUrlByIdsAsync(IEnumerable<int> schoolIds, string lang)
        {
            return await _schoolCatalogRepository.GetSchoolImageIdAndNameAndUrlByIdsAsync(schoolIds, lang);
        }
    }
}
