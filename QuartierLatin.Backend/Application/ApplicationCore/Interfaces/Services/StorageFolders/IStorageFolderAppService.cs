using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Models;
using QuartierLatin.Backend.Application.ApplicationCore.Models.FolderModels;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.StorageFolders
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
        Task<int> GetChatFolderIdAsync();
    }
}
