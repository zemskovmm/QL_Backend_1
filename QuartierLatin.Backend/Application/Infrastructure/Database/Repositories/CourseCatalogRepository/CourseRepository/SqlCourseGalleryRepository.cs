using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CourseCatalogRepository.CourseRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CourseCatalogModels.CoursesModels;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Repositories.CourseCatalogRepository.CourseRepository
{
    public class SqlCourseGalleryRepository : ICourseGalleryRepository
    {
        private readonly AppDbContextManager _db;

        public SqlCourseGalleryRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task CreateGalleryItemToCourseAsync(int courseId, int imageId)
        {
            await _db.ExecAsync(db => db.InsertAsync(new CourseGallery
            {
                ImageId = imageId,
                CourseId = courseId
            }));
        }

        public async Task DeleteGalleryItemToCourseAsync(int courseId, int imageId)
        {
            await _db.ExecAsync(db =>
                db.CourseGalleries
                    .Where(gallery => gallery.CourseId == courseId && gallery.ImageId == imageId)
                    .DeleteAsync());
        }

        public async Task<List<int>> GetGalleryToCourseAsync(int courseId)
        {
            return await _db.ExecAsync(db =>
                db.CourseGalleries.Where(gallery => gallery.CourseId == courseId)
                    .Select(gallery => gallery.ImageId).ToListAsync());
        }
    }
}
