using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Models.HousingModels;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.HousingRepositories
{
    public interface IHousingRepository
    {
        Task<int> CreateHousingAsync(int? price, int? imageId, List<HousingLanguage> housingLanguage);
        Task<(Housing housing, Dictionary<int, HousingLanguage> housingLanguage)> GetHousingByIdAsync(int id);
        Task<List<(Housing housing, Dictionary<int, HousingLanguage> housingLanguage)>> GetHousingListAsync();
        Task UpdateHousingByIdAsync(int id, int? price, int? imageId);
        Task CreateOrUpdateHousingLanguageByIdAsync(int housingId, string htmlDescription, int languageId, string name, string url, JObject? metadata, JObject? location);
        Task<(Housing housing, Dictionary<int, HousingLanguage> housingLanguage)> GetHousingByUrlWithLanguageAsync(int languageId, string url);
        Task<(int totalItems, List<(Housing housing, HousingLanguage housingLanguage)> housingAndLanguage)> GetHousingPageByFilter(List<List<int>> commonTraitsIds, int langId, List<int> priceIds, int skip, int take);
    }
}
