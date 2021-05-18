using System.Collections.Generic;
using QuartierLatin.Backend.Models.FolderModels;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Models.Repositories.StorageFoldersRepositories
{
    public interface IStorageFoldersRepository
    {
        Task<int> CreateStorageFolderASync(string folderName, int? folderParentId);
        Task<StorageFolder> GetStorageFolderByIdAsync(int id);
        Task<List<Blob>> GetBlobFromFolderAsync(int id);
        Task RemoveStorageFolderAsync(int id);
        Task<List<Blob>> GetFilesInfoInDefaultFolderAsync();
        Task<List<StorageFolder>> GetChildFoldersAsync(int storageFolderId);
        Task<List<StorageFolder>> GetDefaultChildFoldersAsync();
        Task UpdateFolderNameAsync(int id, string title);
    }
}
