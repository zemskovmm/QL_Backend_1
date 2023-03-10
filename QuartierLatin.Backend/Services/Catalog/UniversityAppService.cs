using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.AppStateRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.Catalog;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;

namespace QuartierLatin.Backend.Services.Catalog
{
    public class UniversityAppService : IUniversityAppService
    {
        private readonly IUniversityRepository _universityRepository;
        private readonly IAppStateEntryRepository _appStateEntryRepository;
        public UniversityAppService(IUniversityRepository universityRepository, ILanguageRepository languageRepository,
            IAppStateEntryRepository appStateEntryRepository)
        {
            _universityRepository = universityRepository;
            _appStateEntryRepository = appStateEntryRepository;
        }

        public async Task<List<(University university, Dictionary<int, UniversityLanguage> universityLanguage)>> GetUniversityListAsync()
        {
            var universityIdList = await _universityRepository.GetUniversityIdListAsync();

            var response = universityIdList.Select(university => GetUniversityByIdAsync(university)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult()).ToList();

            return response;
        }

        public async Task<(University university, Dictionary<int, UniversityLanguage> universityLanguage)> GetUniversityByIdAsync(int id)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(id);

            var universityLanguageDictionary =
                await _universityRepository.GetUniversityLanguageByUniversityIdAsync(university.Id);

            return (university : university, universityLanguage: universityLanguageDictionary);
        }

        public async Task UpdateUniversityByIdAsync(int id, int? foundationYear, int? logoId, int? bannerId)
        {
            await _universityRepository.UpdateUniversityAsync(id, foundationYear, logoId, bannerId);
            await _appStateEntryRepository.UpdateLastChangeTimeAsync();
        }

        public async Task UpdateUniversityLanguageByIdAsync(int id, string description, int languageId, string name,
            string url, JObject? metadata)
        {
            await _universityRepository.UpdateUniversityLanguageByIdAsync(id, description, languageId, name, url, metadata);
            await _appStateEntryRepository.UpdateLastChangeTimeAsync();
        }

        public async Task<int> CreateUniversityAsync(int? foundationYear, int? logoId, int? bannerId, List<UniversityLanguage> universityLanguage)
        {

            var res = await _universityRepository.CreateUniversityAsync(foundationYear, logoId, bannerId, universityLanguage);
            await _appStateEntryRepository.UpdateLastChangeTimeAsync();
            return res;
        }

        public Task<List<Specialty>> GetSpecialtiesUniversityByUniversityId(int universityId) =>
            _universityRepository.GetSpecialtiesUniversityByUniversityIdList(universityId);

        public async Task<(University university, Dictionary<int, UniversityLanguage> universityLanguage)> GetUniversityByUrlWithLanguage(int languageId, string url)
        {
            var id = await _universityRepository.GetUniversityIdByUrlAndLanguage(languageId, url);

            var university = await _universityRepository.GetUniversityByIdAsync(id);

            var universityLanguage = await _universityRepository.GetUniversityLanguageByUniversityIdAsync(id);

            return (university: university, universityLanguage: universityLanguage);
        }
    }
}