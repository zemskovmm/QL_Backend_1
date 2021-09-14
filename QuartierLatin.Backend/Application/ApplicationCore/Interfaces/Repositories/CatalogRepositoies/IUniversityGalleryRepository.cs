using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CatalogRepositoies
{
    public interface IUniversityGalleryRepository
    {
        Task<List<int>> GetGalleryToUniversityAsync(int universityId);
        Task CreateGalleryItemToUniversityAsync(int universityId, int imageId);
        Task DeleteGalleryItemToUniversityAsync(int universityId, int imageId);
    }
}
