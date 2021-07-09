﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.Interfaces.Catalog;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Models.Repositories.AppStateRepository;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;

namespace QuartierLatin.Backend.Application.Catalog
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

        public async Task UpdateUniversityByIdAsync(int id, int? foundationYear)
        {
            await _universityRepository.UpdateUniversityAsync(id, foundationYear);
            await _appStateEntryRepository.UpdateLastChangeTimeAsync();
        }

        public async Task UpdateUniversityLanguageByIdAsync(int id, string description, int languageId, string name,
            string url, JObject? metadata)
        {
            await _universityRepository.CreateOrUpdateUniversityLanguageAsync(id, languageId, name, description, url, metadata);
            await _appStateEntryRepository.UpdateLastChangeTimeAsync();
        }

        public async Task<int> CreateUniversityAsync(int? universityFoundationYear)
        {
            return await _universityRepository.CreateUniversityAsync(universityFoundationYear);
            await _appStateEntryRepository.UpdateLastChangeTimeAsync();
        }

        public async Task CreateUniversityLanguageListAsync(List<UniversityLanguage> universityLanguage)
        {
            await _universityRepository.CreateUniversityLanguageListAsync(universityLanguage);
            await _appStateEntryRepository.UpdateLastChangeTimeAsync();
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