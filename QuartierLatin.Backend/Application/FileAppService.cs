using Microsoft.AspNetCore.Http;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Storages;
using System;
using System.IO;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application
{
    public class FileAppService : IFileAppService
    {
        private readonly IBlobFileStorage _blobFileStorage;
        private readonly IBlobRepository _blobRepository;

        public FileAppService(IBlobFileStorage blobFileStorage, IBlobRepository blobRepository)
        {
            _blobFileStorage = blobFileStorage;
            _blobRepository = blobRepository;
        }
        public async Task<long> UploadFileAsync(IFormFile file, string fileType)
        {
            var newId = await _blobRepository.CreateBlobIdAsync(fileType, file.FileName);
            await _blobFileStorage.CreateBlobAsync(newId, file.OpenReadStream());

            return newId;
        }

        public async Task<(Stream, string, string)> GetFileAsync(long id)
        {
            var stream = _blobFileStorage.OpenBlob(id);
            var fileRecord = await _blobRepository.GetBlobInfoAsync(id);

            return (stream, fileRecord.FileType, fileRecord.OriginalFileName);
        }

        public async Task DeleteFileAsync(long id)
        {
            await _blobRepository.DeleteBlobAsync(id);
            await _blobFileStorage.DeleteBlob(id);
        }
    }
}
