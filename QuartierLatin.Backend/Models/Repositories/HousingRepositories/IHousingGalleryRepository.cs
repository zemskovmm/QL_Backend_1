using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Models.Repositories.HousingRepositories
{
    public interface IHousingGalleryRepository
    {
        Task<List<int>> GetGalleryToHousingAsync(int housingId);
        Task CreateGalleryItemToHousingAsync(int housingId, int imageId);
        Task DeleteGalleryItemToHousingAsync(int housingId, int imageId);
    }
}
