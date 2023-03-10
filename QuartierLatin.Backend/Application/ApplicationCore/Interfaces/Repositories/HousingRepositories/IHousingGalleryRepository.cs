using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.HousingRepositories
{
    public interface IHousingGalleryRepository
    {
        Task<List<int>> GetGalleryToHousingAsync(int housingId);
        Task CreateGalleryItemToHousingAsync(int housingId, int imageId);
        Task DeleteGalleryItemToHousingAsync(int housingId, int imageId);
        Task<Dictionary<int, List<int>>> GetGalleriesByHousingIdsAsync(IEnumerable<int> housingIds);
    }
}
