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
    }
}
