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

        public async Task<int> CreateBlobIdAsync(string fileType, string originalFileName, int? storageFolderId = null)
        {
            return await _db.ExecAsync(db => db.InsertWithInt32IdentityAsync(new Blob {FileType = fileType, OriginalFileName = originalFileName, StorageFolderId = storageFolderId}));
        }

        public async Task DeleteBlobAsync(int id) =>
            await _db.ExecAsync(db => db.Blobs.Select(x => x.Id == id).DeleteAsync());

        public async Task EditBlobAsync(int id, string fileType)
        {
            await _db.ExecAsync(db => db.UpdateAsync(new Blob
            {
                Id = id,
                FileType = fileType
            }));
        }

        public async Task<Blob> GetBlobInfoAsync(int id)
        {
            return await _db.ExecAsync(db => db.Blobs.FirstOrDefaultAsync(blob => blob.Id == id));
        }
    }
}