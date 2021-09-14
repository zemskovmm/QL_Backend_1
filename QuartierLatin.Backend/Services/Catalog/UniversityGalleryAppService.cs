using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.Catalog;

namespace QuartierLatin.Backend.Services.Catalog
{
    public class UniversityGalleryAppService : IUniversityGalleryAppService
    {
        private readonly IUniversityGalleryRepository _universityGalleryRepository;

        public UniversityGalleryAppService(IUniversityGalleryRepository universityGalleryRepository)
        {
            _universityGalleryRepository = universityGalleryRepository;
        }

        public async Task<List<int>> GetGalleryToUniversityAsync(int universityId)
        {
            return await _universityGalleryRepository.GetGalleryToUniversityAsync(universityId);
        }

        public async Task CreateGalleryItemToUniversityAsync(int universityId, int imageId)
        {
            await _universityGalleryRepository.CreateGalleryItemToUniversityAsync(universityId, imageId);
        }

        public async Task DeleteGalleryItemToUniversityAsync(int universityId, int imageId)
        {
            await _universityGalleryRepository.DeleteGalleryItemToUniversityAsync(universityId, imageId);
        }
    }
}
