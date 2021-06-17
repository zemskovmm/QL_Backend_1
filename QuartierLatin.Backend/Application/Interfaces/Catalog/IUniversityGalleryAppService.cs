using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.Interfaces.Catalog
{
    public interface IUniversityGalleryAppService
    {
        Task<List<int>> GetGalleryToUniversityAsync(int universityId);
        Task CreateGalleryItemToUniversityAsync(int universityId, int imageId);
        Task DeleteGalleryItemToUniversityAsync(int universityId, int imageId);
    }
}
