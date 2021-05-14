using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.FolderModels;
using QuartierLatin.Backend.Models.Repositories.StorageFoldersRepositories;

namespace QuartierLatin.Backend.Database.Repositories.StorageFolders
{
    public class SqlStorageFoldersRepository : IStorageFoldersRepository
    {
        private readonly AppDbContextManager _db;

        public SqlStorageFoldersRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<int> CreateStorageFolderASync(string folderName, int? folderParentId)
        {
            return await _db.ExecAsync(db => db.InsertWithInt32IdentityAsync(new StorageFolder
            {
                FolderName = folderName,
                FolderParentId = folderParentId
            }));
        }

        public async Task<StorageFolder> GetStorageFolderByIdAsync(int id)
        {
            return await _db.ExecAsync(db => db.StorageFolders.FirstOrDefaultAsync(folder => folder.Id == id));
        }

        public async Task<List<Blob>> GetBlobFromFolderAsync(int id)
        {
            return await _db.ExecAsync(db => db.Blobs.Where(blob => blob.StorageFolderId == id).ToListAsync());
        }
    }
}
