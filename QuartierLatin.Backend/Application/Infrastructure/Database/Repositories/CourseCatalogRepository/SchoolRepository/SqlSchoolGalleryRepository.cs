using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CourseCatalogRepository.SchoolRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CourseCatalogModels.SchoolModels;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Repositories.CourseCatalogRepository.SchoolRepository
{
    public class SqlSchoolGalleryRepository : ISchoolGalleryRepository
    {
        private readonly AppDbContextManager _db;

        public SqlSchoolGalleryRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task CreateGalleryItemToSchoolAsync(int schoolId, int imageId)
        {
            await _db.ExecAsync(db => db.InsertAsync(new SchoolGallery
            {
                ImageId = imageId,
                SchoolId = schoolId
            }));
        }

        public async Task DeleteGalleryItemToSchoolAsync(int schoolId, int imageId)
        {
            await _db.ExecAsync(db =>
                db.SchoolGalleries
                    .Where(gallery => gallery.SchoolId == schoolId && gallery.ImageId == imageId)
                    .DeleteAsync());
        }

        public async Task<List<int>> GetGalleryToSchoolAsync(int schoolId)
        {
            return await _db.ExecAsync(db =>
                db.SchoolGalleries.Where(gallery => gallery.SchoolId == schoolId)
                    .Select(gallery => gallery.ImageId).ToListAsync());
        }
    }
}