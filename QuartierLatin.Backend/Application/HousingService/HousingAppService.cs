using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.Interfaces.HousingServices;
using QuartierLatin.Backend.Models.HousingModels;
using QuartierLatin.Backend.Models.Repositories.HousingRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.HousingService
{
    public class HousingAppService : IHousingAppService
    {
        private readonly IHousingRepository _housingRepository;

        public HousingAppService(IHousingRepository housingRepository)
        {
            _housingRepository = housingRepository;
        }

        public async Task<int> CreateHousingAsync(int? price)
        {
            return await _housingRepository.CreateHousingAsync(price);
        }

        public async Task CreateHousingLanguageListAsync(List<HousingLanguage> housingLanguage)
        {
            await _housingRepository.CreateHousingLanguageListAsync(housingLanguage);
        }

        public async Task<(Housing housing, Dictionary<int, HousingLanguage> housingLanguage)> GetHousingByIdAsync(int id)
        {
            return await _housingRepository.GetHousingByIdAsync(id);
        }

        public async Task<List<(Housing housing, Dictionary<int, HousingLanguage> housingLanguage)>> GetHousingListAsync()
        {
            return await _housingRepository.GetHousingListAsync();
        }

        public async Task UpdateHousingByIdAsync(int id, int? price)
        {
            await _housingRepository.UpdateHousingByIdAsync(id, price);
        }

        public async Task CreateOrUpdateHousingLanguageByIdAsync(int housingId, string htmlDescription, int languageId, string name, string url, JObject metadata)
        {
            await _housingRepository.CreateOrUpdateHousingLanguageByIdAsync(housingId, htmlDescription, languageId, name, url,
                metadata);
        }
    }
}
