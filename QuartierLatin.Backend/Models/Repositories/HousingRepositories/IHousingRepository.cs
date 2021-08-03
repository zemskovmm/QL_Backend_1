using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.HousingModels;

namespace QuartierLatin.Backend.Models.Repositories.HousingRepositories
{
    public interface IHousingRepository
    {
        Task<int> CreateHousingAsync(int? price);
        Task CreateHousingLanguageListAsync(List<HousingLanguage> housingLanguage);
        Task<(Housing housing, Dictionary<int, HousingLanguage> housingLanguage)> GetHousingByIdAsync(int id);
        Task<List<(Housing housing, Dictionary<int, HousingLanguage> housingLanguage)>> GetHousingListAsync();
        Task UpdateHousingByIdAsync(int id, int? price);
        Task CreateOrUpdateHousingLanguageByIdAsync(int housingId, string htmlDescription, int languageId, string name, string url, JObject metadata);
    }
}
