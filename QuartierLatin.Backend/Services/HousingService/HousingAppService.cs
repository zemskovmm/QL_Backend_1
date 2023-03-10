using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.HousingRepositories;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.HousingServices;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.HousingModels;

namespace QuartierLatin.Backend.Services.HousingService
{
    public class HousingAppService : IHousingAppService
    {
        private readonly IHousingRepository _housingRepository;
        private readonly ICommonTraitRepository _commonTraitRepository;
        public HousingAppService(IHousingRepository housingRepository, ICommonTraitRepository commonTraitRepository)
        {
            _housingRepository = housingRepository;
            _commonTraitRepository = commonTraitRepository;
        }

        public async Task<int> CreateHousingAsync(int? price, int? imageId, List<HousingLanguage> housingLanguage)
        {
            return await _housingRepository.CreateHousingAsync(price, imageId, housingLanguage);
        }

        public async Task<(Housing housing, Dictionary<int, HousingLanguage> housingLanguage)> GetHousingByIdAsync(int id)
        {
            return await _housingRepository.GetHousingByIdAsync(id);
        }

        public async Task<List<(Housing housing, Dictionary<int, HousingLanguage> housingLanguage)>> GetHousingListAsync()
        {
            return await _housingRepository.GetHousingListAsync();
        }

        public async Task UpdateHousingByIdAsync(int id, int? price, int? imageId)
        {
            await _housingRepository.UpdateHousingByIdAsync(id, price, imageId);
        }

        public async Task CreateOrUpdateHousingLanguageByIdAsync(int housingId, string htmlDescription, int languageId, string name, string url, JObject? metadata, JObject? location)
        {
            await _housingRepository.CreateOrUpdateHousingLanguageByIdAsync(housingId, htmlDescription, languageId, name, url,
                metadata, location);
        }

        public async Task<(Housing housing, Dictionary<int, HousingLanguage> housingLanguage)> GetHousingByUrlWithLanguageAsync(int languageId, string url)
        {
            return await _housingRepository.GetHousingByUrlWithLanguageAsync(languageId, url);
        }

        public async Task<Dictionary<int, List<CommonTrait>>> GetCommonTraitListByHousingIdsAsync(IEnumerable<int> housingIds)
        {
            return await _commonTraitRepository.GetCommonTraitListByHousingIdsAsync(housingIds);
        }
    }
}
