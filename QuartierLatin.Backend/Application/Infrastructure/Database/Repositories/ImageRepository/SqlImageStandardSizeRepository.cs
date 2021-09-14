using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.ImageRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Models.ImageStandardSizeModels;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Repositories.ImageRepository
{
    public class SqlImageStandardSizeRepository : IImageStandardSizeRepository
    {
        private readonly AppDbContextManager _db;

        public SqlImageStandardSizeRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<ImageStandardSize> GetImageStandardSizeByIdAsync(int id)
        {
            return await _db.ExecAsync(db =>
                db.ImageStandardSizes.FirstOrDefaultAsync(imageStandard => imageStandard.Id == id));
        }

        public async Task<List<ImageStandardSize>> GetImageStandardSizeListAsync()
        {
            return await _db.ExecAsync(db => db.ImageStandardSizes.ToListAsync());
        }

        public async Task<int> CreateImageStandardSizeAsync(int height, int width)
        {
            return await _db.ExecAsync(db => db.InsertWithInt32IdentityAsync(new ImageStandardSize
            {
                Height = height,
                Width = width
            }));
        }

        public async Task UpdateImageStandardSizeByIdAsync(int id, int height, int width)
        {
            await _db.ExecAsync(db => db.UpdateAsync(new ImageStandardSize
            {
                Id = id,
                Height = height,
                Width = width
            }));
        }

        public async Task DeleteImageStandardSizeByIdAsync(int id)
        {
            await _db.ExecAsync(db =>
                db.ImageStandardSizes.Select(imageStandard => imageStandard.Id == id).DeleteAsync());
        }
    }
}
