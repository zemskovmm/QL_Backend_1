using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.Interfaces.CourseCatalog.SchoolCatalog;
using QuartierLatin.Backend.Models.Repositories.CourseCatalogRepository.SchoolRepository;

namespace QuartierLatin.Backend.Application.CourseCatalog.SchoolCatalog
{
    public class SchoolGalleryAppService : ISchoolGalleryAppService
    {
        private readonly ISchoolGalleryRepository _schoolGalleryRepository;

        public SchoolGalleryAppService(ISchoolGalleryRepository schoolGalleryRepository)
        {
            _schoolGalleryRepository = schoolGalleryRepository;
        }

        public async Task CreateGalleryItemToSchoolAsync(int schoolId, int imageId)
        {
            await _schoolGalleryRepository.CreateGalleryItemToSchoolAsync(schoolId, imageId);
        }

        public async Task DeleteGalleryItemToSchoolAsync(int schoolId, int imageId)
        {
            await _schoolGalleryRepository.DeleteGalleryItemToSchoolAsync(schoolId, imageId);
        }

        public async Task<List<int>> GetGalleryToSchoolAsync(int schoolId)
        {
            return await _schoolGalleryRepository.GetGalleryToSchoolAsync(schoolId);
        }
    }
}
