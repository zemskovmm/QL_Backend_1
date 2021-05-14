using System.Collections.Generic;
using QuartierLatin.Backend.Models.FolderModels;
using System.Threading.Tasks;
using Microsoft.Graph;
using QuartierLatin.Backend.Models;

namespace QuartierLatin.Backend.Application.Interfaces.StorageFolders
{
    public interface IStorageFolderAppService
    {
        Task<int> CreateStorageFolderAsync(string folderName, int? folderParentId);
        Task<StorageFolder> GetStorageFolderByIdAsync(int id);
        Task<List<Blob>> GetFilesInfoInFolderAsync(int id);
    }
}
