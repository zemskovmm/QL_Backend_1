using System.Collections.Generic;
using QuartierLatin.Backend.Application.Interfaces.StorageFolders;
using QuartierLatin.Backend.Models.Repositories.StorageFoldersRepositories;
using System.Threading.Tasks;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.FolderModels;

namespace QuartierLatin.Backend.Application.StorageFolders
{
    public class StorageFolderAppService : IStorageFolderAppService
    {
        private readonly IStorageFoldersRepository _storageFoldersRepository;

        public StorageFolderAppService(IStorageFoldersRepository storageFoldersRepository)
        {
            _storageFoldersRepository = storageFoldersRepository;
        }

        public async Task<int> CreateStorageFolderAsync(string folderName, int? folderParentId)
        {
            return await _storageFoldersRepository.CreateStorageFolderASync(folderName, folderParentId);
        }

        public async Task<StorageFolder> GetStorageFolderByIdAsync(int id)
        {
            return await _storageFoldersRepository.GetStorageFolderByIdAsync(id);
        }

        public async Task<List<Blob>> GetFilesInfoInFolderAsync(int id)
        {
            return await _storageFoldersRepository.GetBlobFromFolderAsync(id);
        }
    }
}
