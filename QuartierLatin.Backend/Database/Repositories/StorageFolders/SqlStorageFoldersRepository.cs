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
            return await _db.ExecAsync(db => db.StorageFolders.FirstOrDefaultAsync(folder => folder.Id == id && folder.IsDeleted == false));
        }

        public async Task<List<Blob>> GetBlobFromFolderAsync(int id)
        {
            return await _db.ExecAsync(db => db.Blobs.Where(blob => blob.StorageFolderId == id).ToListAsync());
        }

        public async Task RemoveStorageFolderAsync(int id)
        { 
            await _db.ExecAsync(async db =>
            {
                var storage = await db.StorageFolders.FirstOrDefaultAsync(folder => folder.Id == id);
                storage.IsDeleted = true;
                await db.UpdateAsync(storage);
            });
        }

        public async Task<List<Blob>> GetFilesInfoInDefaultFolderAsync()
        {
            return await _db.ExecAsync(db => db.Blobs.Where(blob => blob.StorageFolderId == null).ToListAsync());
        }

        public async Task<List<StorageFolder>> GetChildFoldersAsync(int storageFolderId)
        {
            return await _db.ExecAsync(db =>
                db.StorageFolders.Where(folder => folder.FolderParentId == storageFolderId && folder.IsDeleted == false)
                    .ToListAsync());
        }

        public async Task<List<StorageFolder>> GetDefaultChildFoldersAsync()
        {
            return await _db.ExecAsync(db =>
                db.StorageFolders.Where(folder => folder.FolderParentId == null && folder.IsDeleted == false)
                    .ToListAsync());
        }

        public async Task UpdateFolderNameAsync(int id, string title)
        {
            await _db.ExecAsync(async db =>
            {
                var storageFolder = await db.StorageFolders.FirstOrDefaultAsync(folder => folder.Id == id);
                storageFolder.FolderName = title;
                await db.UpdateAsync(storageFolder);
            });
        }
    }
}
