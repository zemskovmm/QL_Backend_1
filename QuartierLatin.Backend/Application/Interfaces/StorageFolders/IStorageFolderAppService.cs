using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.FolderModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.Interfaces.StorageFolders
{
    public interface IStorageFolderAppService
    {
        Task<int> CreateStorageFolderAsync(string folderName, int? folderParentId);
        Task<StorageFolder> GetStorageFolderByIdAsync(int id);
        Task<List<Blob>> GetFilesInfoInFolderAsync(int id);
        Task RemoveStorageFolderAsync(int id);
        Task<List<Blob>> GetFilesInfoInDefaultFolderAsync();
        Task<List<StorageFolder>> GetChildFoldersAsync(int storageFolderId);
        Task<List<StorageFolder>> GetDefaultChildFoldersAsync();
        Task UpdateFolderNameAsync(int id, string title);
    }
}
