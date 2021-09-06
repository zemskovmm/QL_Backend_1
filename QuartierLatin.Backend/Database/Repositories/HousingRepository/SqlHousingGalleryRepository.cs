using LinqToDB;
using QuartierLatin.Backend.Models.HousingModels;
using QuartierLatin.Backend.Models.Repositories.HousingRepositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Database.Repositories.HousingRepository
{
    public class SqlHousingGalleryRepository : IHousingGalleryRepository
    {
        private readonly AppDbContextManager _db;

        public SqlHousingGalleryRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task CreateGalleryItemToHousingAsync(int housingId, int imageId)
        {
            await _db.ExecAsync(db => db.InsertAsync(new HousingGallery
            {
                ImageId = imageId,
                HousingId = housingId
            }));
        }

        public async Task DeleteGalleryItemToHousingAsync(int housingId, int imageId)
        {
            await _db.ExecAsync(db =>
                db.HousingGalleries
                    .Where(gallery => gallery.HousingId == housingId && gallery.ImageId == imageId)
                    .DeleteAsync());
        }

        public async Task<Dictionary<int, List<int>>> GetGalleriesByHousingIdsAsync(IEnumerable<int> housingIds)
        {
            return await _db.ExecAsync(async db =>
            {
                var query = from housing in db.Housings where housingIds.Contains(housing.Id)
                    let gallery = db.HousingGalleries.Where(gallery => gallery.HousingId == housing.Id)
                    select new { housing.Id, gallery };

                return query.ToDictionary(gallery => gallery.Id, gallery =>
                    gallery.gallery.Select(gallery => gallery.ImageId).ToList());
            });
        }

        public async Task<List<int>> GetGalleryToHousingAsync(int housingId)
        {
            return await _db.ExecAsync(db =>
                db.HousingGalleries.Where(gallery => gallery.HousingId == housingId)
                    .Select(gallery => gallery.ImageId).ToListAsync());
        }
    }
}