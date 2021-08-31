﻿using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.Interfaces.HousingServices;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.HousingModels;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Models.Repositories.HousingRepositories;

namespace QuartierLatin.Backend.Application.HousingService
{
    public class HousingAccommodationTypeAppService : IHousingAccommodationTypeAppService
    {
        private readonly IHousingAccommodationTypeRepository _housingAccommodationTypeRepository;
        private readonly ICommonTraitRepository _commonTraitRepository;

        public HousingAccommodationTypeAppService(IHousingAccommodationTypeRepository housingAccommodationTypeRepository, ICommonTraitRepository commonTraitRepository)
        {
            _housingAccommodationTypeRepository = housingAccommodationTypeRepository;
            _commonTraitRepository = commonTraitRepository;
        }

        public async Task<List<HousingAccommodationType>> GetHousingAccommodationTypeListAsync()
        {
            return await _housingAccommodationTypeRepository.GetHousingAccommodationTypeListAsync();
        }

        public async Task<int> CreateHousingAccommodationTypeAsync(Dictionary<string, string> names, int housingId, int price, string residents, string square)
        {
            return await _housingAccommodationTypeRepository.CreateHousingAccommodationTypeAsync(names, housingId, price, residents, square);
        }

        public async Task<HousingAccommodationType> GetHousingAccommodationTypeByIdAsync(int id)
        {
            return await _housingAccommodationTypeRepository.GetHousingAccommodationTypeByIdAsync(id);
        }

        public async Task UpdateHousingAccommodationTypeByIdAsync(int id, Dictionary<string, string> names, int housingId, int price, string residents, string square)
        {
            await _housingAccommodationTypeRepository.UpdateHousingAccommodationTypeByIdAsync(id, names, housingId, price, residents, square);
        }

        public async Task<List<HousingAccommodationType>> GetHousingAccommodationTypeListByHousingIdAsync(int housingId)
        {
            return await _housingAccommodationTypeRepository.GetHousingAccommodationTypeListByHousingIdAsync(housingId);
        }

        public async Task<Dictionary<int, List<CommonTrait>>> GetCommonTraitListByHousingAccommodationTypeIdsAsync(IEnumerable<int> housingAccommodationIds)
        {
            return await _commonTraitRepository.GetCommonTraitListByHousingAccommodationTypeIdsAsync(housingAccommodationIds);
        }
    }
}
