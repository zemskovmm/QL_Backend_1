using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.HousingModels;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.HousingServices
{
    public interface IHousingAccommodationTypeAppService
    {
        Task<List<HousingAccommodationType>> GetHousingAccommodationTypeListAsync();
        Task<int> CreateHousingAccommodationTypeAsync(Dictionary<string, string> names, int housingId, int price, string residents, string square);
        Task<HousingAccommodationType> GetHousingAccommodationTypeByIdAsync(int id);
        Task UpdateHousingAccommodationTypeByIdAsync(int id, Dictionary<string, string> names, int housingId, int price, string residents, string square);
        Task<List<HousingAccommodationType>> GetHousingAccommodationTypeListByHousingIdAsync(int housingId);
        Task<Dictionary<int, List<CommonTrait>>> GetCommonTraitListByHousingAccommodationTypeIdsAsync(IEnumerable<int> housingAccommodationIds);
    }
}
