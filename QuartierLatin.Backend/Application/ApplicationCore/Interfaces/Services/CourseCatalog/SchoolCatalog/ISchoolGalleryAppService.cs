using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.CourseCatalog.SchoolCatalog
{
    public interface ISchoolGalleryAppService
    {
        Task CreateGalleryItemToSchoolAsync(int schoolId, int imageId);
        Task DeleteGalleryItemToSchoolAsync(int schoolId, int imageId);
        Task<List<int>> GetGalleryToSchoolAsync(int schoolId);
    }
}
