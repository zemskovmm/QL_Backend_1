using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Models.Repositories.CatalogRepositoies
{
    public interface IUniversityGalleryRepository
    {
        Task<List<int>> GetGalleryToUniversityAsync(int universityId);
        Task CreateGalleryItemToUniversityAsync(int universityId, int imageId);
        Task DeleteGalleryItemToUniversityAsync(int universityId, int imageId);
    }
}
