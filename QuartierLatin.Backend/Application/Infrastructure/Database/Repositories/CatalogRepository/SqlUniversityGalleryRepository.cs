using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Repositories.CatalogRepository
{
    public class SqlUniversityGalleryRepository : IUniversityGalleryRepository
    {
        private readonly AppDbContextManager _db;

        public SqlUniversityGalleryRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<List<int>> GetGalleryToUniversityAsync(int universityId)
        {
            return await _db.ExecAsync(db =>
                db.UniversityGalleries.Where(gallery => gallery.UniversityId == universityId)
                    .Select(gallery => gallery.ImageId).ToListAsync());
        }

        public async Task CreateGalleryItemToUniversityAsync(int universityId, int imageId)
        {
            await _db.ExecAsync(db => db.InsertAsync(new UniversityGallery
            {
                ImageId = imageId,
                UniversityId = universityId
            }));
        }

        public async Task DeleteGalleryItemToUniversityAsync(int universityId, int imageId)
        {
            await _db.ExecAsync(db =>
                db.UniversityGalleries
                    .Where(gallery => gallery.UniversityId == universityId && gallery.ImageId == imageId)
                    .DeleteAsync());
        }
    }
}
