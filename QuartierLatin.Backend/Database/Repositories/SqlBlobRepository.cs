using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.Repositories;
using LinqToDB;

namespace QuartierLatin.Backend.Database.Repositories
{
    public class SqlBlobRepository : IBlobRepository
    {
        private readonly AppDbContextManager _db;

        public SqlBlobRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<long> CreateBlobIdAsync(string fileType, string originalFileName)
        {
            return await _db.ExecAsync(db => db.InsertWithInt64IdentityAsync(new Blob {FileType = fileType, OriginalFileName = originalFileName}));
        }

        public async Task DeleteBlobAsync(long id) =>
            await _db.ExecAsync(db => db.Blobs.Select(x => x.Id == id).DeleteAsync());

        public async Task EditBlobAsync(long id, string fileType)
        {
            await _db.ExecAsync(db => db.UpdateAsync(new Blob
            {
                Id = id,
                FileType = fileType
            }));
        }

        public async Task<Blob> GetBlobInfoAsync(long id)
        {
            return await _db.ExecAsync(db => db.Blobs.FirstOrDefaultAsync(blob => blob.Id == id));
        }

        public async Task<Blob> GetBlobInfoByFileNameAndFileTypeAsync(string fileName, string fileType)
        {
            return await _db.ExecAsync(db => db.Blobs.FirstOrDefaultAsync(blob => blob.OriginalFileName == fileName && blob.FileType == fileType));
        }
    }
}