using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.HousingModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.Interfaces.HousingServices
{
    public interface IHousingAppService
    {
        Task<List<(Housing housing, Dictionary<int, HousingLanguage> housingLanguage)>> GetHousingListAsync();
        Task<int> CreateHousingAsync(int? price);
        Task CreateHousingLanguageListAsync(List<HousingLanguage> housingLanguage);
        Task<(Housing housing, Dictionary<int, HousingLanguage> housingLanguage)> GetHousingByIdAsync(int id);
        Task UpdateHousingByIdAsync(int id, int? price);
        Task CreateOrUpdateHousingLanguageByIdAsync(int housingId, string htmlDescription, int languageId, string name, string url, JObject? metadata);
    }
}
