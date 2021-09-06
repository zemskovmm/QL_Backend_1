using QuartierLatin.Backend.Application.Interfaces.HousingServices;
using QuartierLatin.Backend.Models.Repositories.HousingRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.HousingService
{
    public class HousingGalleryAppService : IHousingGalleryAppService
    {
        private readonly IHousingGalleryRepository _housingGalleryRepository;

        public HousingGalleryAppService(IHousingGalleryRepository housingGalleryRepository)
        {
            _housingGalleryRepository = housingGalleryRepository;
        }

        public async Task<List<int>> GetGalleryToHousingAsync(int housingId)
        {
            return await _housingGalleryRepository.GetGalleryToHousingAsync(housingId);
        }

        public async Task CreateGalleryItemToHousingAsync(int housingId, int imageId)
        {
            await _housingGalleryRepository.CreateGalleryItemToHousingAsync(housingId, imageId);
        }

        public async Task DeleteGalleryItemToHousingAsync(int housingId, int imageId)
        {
            await _housingGalleryRepository.DeleteGalleryItemToHousingAsync(housingId, imageId);
        }

        public async Task<Dictionary<int, List<int>>> GetGalleriesByHousingIdsAsync(IEnumerable<int> housingIds)
        {
            return await _housingGalleryRepository.GetGalleriesByHousingIdsAsync(housingIds);
        }
    }
}
