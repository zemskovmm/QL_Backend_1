using QuartierLatin.Backend.Models.HousingModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Models.Repositories.HousingRepositories
{
    public interface IHousingAccommodationTypeRepository
    {
        Task<List<HousingAccommodationType>> GetHousingAccommodationTypeListAsync();
        Task<int> CreateHousingAccommodationTypeAsync(Dictionary<string, string> names, int housingId, int price, string residents, string square);
        Task<HousingAccommodationType> GetHousingAccommodationTypeByIdAsync(int id);
        Task UpdateHousingAccommodationTypeByIdAsync(int id, Dictionary<string, string> names, int housingId, int price, string residents, string square);
        Task<List<HousingAccommodationType>> GetHousingAccommodationTypeListByHousingIdAsync(int housingId);
    }
}