﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.Interfaces.Catalog;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;

namespace QuartierLatin.Backend.Application.Catalog
{
    public class UniversityAppService : IUniversityAppService
    {
        private readonly IUniversityRepository _universityRepository;

        public UniversityAppService(IUniversityRepository universityRepository, ILanguageRepository languageRepository)
        {
            _universityRepository = universityRepository;
        }

        public async Task<List<(University, Dictionary<int, UniversityLanguage>)>> GetUniversityListAsync()
        {
            var universityIdList = await _universityRepository.GetUniversityIdListAsync();

            var response = universityIdList.Select(university => GetUniversityByIdAsync(university)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult()).ToList();

            return response;
        }

        public async Task<(University, Dictionary<int, UniversityLanguage>)> GetUniversityByIdAsync(int id)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(id);

            var universityLanguageDictionary =
                await _universityRepository.GetUniversityLanguageByUniversityIdAsync(university.Id);

            return (university, universityLanguageDictionary);
        }

        public async Task UpdateUniversityByIdAsync(int id, int? foundationYear, string website)
        {
            await _universityRepository.UpdateUniversityAsync(id, foundationYear, website);
        }

        public async Task UpdateUniversityLanguageByIdAsync(int id, string description, int languageId, string name,
            string url)
        {
            await _universityRepository.CreateOrUpdateUniversityLanguageAsync(id, languageId, name, description, url);
        }

        public async Task<int> CreateUniversityAsync(int? universityFoundationYear, string universityWebsite)
        {
            return await _universityRepository.CreateUniversityAsync(universityFoundationYear, universityWebsite);
        }

        public async Task CreateUniversityLanguageListAsync(List<UniversityLanguage> universityLanguage)
        {
            await _universityRepository.CreateUniversityLanguageListAsync(universityLanguage);
        }

        public async Task<(University, Dictionary<int, UniversityLanguage>)> GetUniversityByUrl(string url)
        {
            var id = await _universityRepository.GetUniversityIdByUrl(url);

            var university = await _universityRepository.GetUniversityByIdAsync(id);

            var universityLanguage = await _universityRepository.GetUniversityLanguageByUniversityIdAsync(id);

            return (university, universityLanguage);
        }

        public async Task<List<UniversityInstructionLanguage>> GetUniversityLanguageInstructionByUniversityId(int universityId)
        {
            return await _universityRepository.GetUniversityLanguageInstructionByUniversityId(universityId);
        }

        public async Task<List<(Specialty, int)>> GetSpecialtiesUniversityByUniversityId(int universityId)
        {
            return await _universityRepository.GetSpecialtiesUniversityByUniversityIdList(universityId);
        }
    }
}