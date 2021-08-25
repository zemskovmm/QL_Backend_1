using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Models.Repositories.CourseCatalogRepository.SchoolRepository
{
    public interface ISchoolGalleryRepository
    {
        Task CreateGalleryItemToSchoolAsync(int schoolId, int imageId);
        Task DeleteGalleryItemToSchoolAsync(int schoolId, int imageId);
        Task<List<int>> GetGalleryToSchoolAsync(int schoolId);
    }
}
