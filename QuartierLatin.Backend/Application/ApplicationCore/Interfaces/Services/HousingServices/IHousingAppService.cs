using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.HousingModels;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.HousingServices
{
    public interface IHousingAppService
    {
        Task<List<(Housing housing, Dictionary<int, HousingLanguage> housingLanguage)>> GetHousingListAsync();
        Task<int> CreateHousingAsync(int? price, int? imageId, List<HousingLanguage> housingLanguage);
        Task<(Housing housing, Dictionary<int, HousingLanguage> housingLanguage)> GetHousingByIdAsync(int id);
        Task UpdateHousingByIdAsync(int id, int? price, int? imageId);
        Task CreateOrUpdateHousingLanguageByIdAsync(int housingId, string htmlDescription, int languageId, string name, string url, JObject? metadata);
        Task<(Housing housing, Dictionary<int, HousingLanguage> housingLanguage)> GetHousingByUrlWithLanguageAsync(int languageId, string url);
        Task<Dictionary<int, List<CommonTrait>>> GetCommonTraitListByHousingIdsAsync(IEnumerable<int> housingIds);
    }
}
