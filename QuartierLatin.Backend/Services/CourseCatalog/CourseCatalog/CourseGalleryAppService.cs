using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CourseCatalogRepository.CourseRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.CourseCatalog.CourseCatalog;

namespace QuartierLatin.Backend.Services.CourseCatalog.CourseCatalog
{
    public class CourseGalleryAppService : ICourseGalleryAppService
    {
        private readonly ICourseGalleryRepository _courseGalleryRepository;
        public CourseGalleryAppService(ICourseGalleryRepository courseGalleryRepository)
        {
            _courseGalleryRepository = courseGalleryRepository;
        }

        public async Task CreateGalleryItemToCourseAsync(int courseId, int imageId)
        {
            await _courseGalleryRepository.CreateGalleryItemToCourseAsync(courseId, imageId);
        }

        public async Task DeleteGalleryItemToCourseAsync(int courseId, int imageId)
        {
            await _courseGalleryRepository.DeleteGalleryItemToCourseAsync(courseId, imageId);
        }

        public async Task<List<int>> GetGalleryToCourseAsync(int courseId)
        {
            return await _courseGalleryRepository.GetGalleryToCourseAsync(courseId);
        }
    }
}
