using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CourseCatalogRepository.CourseRepository
{
    public interface ICourseGalleryRepository
    {
        Task CreateGalleryItemToCourseAsync(int courseId, int imageId);
        Task DeleteGalleryItemToCourseAsync(int courseId, int imageId);
        Task<List<int>> GetGalleryToCourseAsync(int courseId);
    }
}
