using LinqToDB;
using QuartierLatin.Backend.Models.CourseCatalogModels.SchoolModels;
using QuartierLatin.Backend.Models.Repositories.CourseCatalogRepository.SchoolRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Database.Repositories.CourseCatalogRepository.SchoolRepository
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
                    .Where(trait => trait.SchoolId == schoolId && trait.ImageId == imageId)
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