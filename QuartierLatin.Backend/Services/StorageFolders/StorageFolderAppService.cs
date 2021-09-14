using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.StorageFoldersRepositories;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.StorageFolders;
using QuartierLatin.Backend.Application.ApplicationCore.Models;
using QuartierLatin.Backend.Application.ApplicationCore.Models.FolderModels;

namespace QuartierLatin.Backend.Services.StorageFolders
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

        public async Task RemoveStorageFolderAsync(int id)
        {
            await _storageFoldersRepository.RemoveStorageFolderAsync(id);
        }

        public async Task<List<Blob>> GetFilesInfoInDefaultFolderAsync()
        {
            return await _storageFoldersRepository.GetFilesInfoInDefaultFolderAsync();
        }

        public async Task<List<StorageFolder>> GetChildFoldersAsync(int storageFolderId)
        {
            return await _storageFoldersRepository.GetChildFoldersAsync(storageFolderId);
        }

        public async Task<List<StorageFolder>> GetDefaultChildFoldersAsync()
        {
            return await _storageFoldersRepository.GetDefaultChildFoldersAsync();
        }

        public async Task UpdateFolderNameAsync(int id, string title)
        {
            await _storageFoldersRepository.UpdateFolderNameAsync(id, title);
        }
    }
}
