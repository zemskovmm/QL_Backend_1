using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Npgsql;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.StorageFoldersRepositories;
using QuartierLatin.Backend.Application.ApplicationCore.Models;
using QuartierLatin.Backend.Application.ApplicationCore.Models.FolderModels;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Repositories.StorageFolders
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
                using var trx = await (db.Connection as NpgsqlConnection).BeginTransactionAsync();
                await db.StorageFolders.Where(folder => folder.Id == id && folder.IsDeleted == false).Set(x=> x.IsDeleted, x => true).UpdateAsync();

                await db.Blobs.Where(blob => blob.StorageFolderId == id)
                    .ForEachAsync(blob => blob.IsDeleted = true);
                await trx.CommitAsync();
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
