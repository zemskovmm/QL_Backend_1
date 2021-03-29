using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Storages;
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
        public async Task<long> UploadFileAsync(Stream file, string fileName, string fileType, int? dimension = null, long? id = null)
        {
            if (dimension is null && id is null)
            {
                var newId = await _blobRepository.CreateBlobIdAsync(fileType, fileName);
                await _blobFileStorage.CreateBlobAsync(newId, file);

                return newId;
            }
            else
            {
                await _blobFileStorage.CreateBlobAsync(id.Value, file, dimension);
                return id.Value;
            }
           
        }

        public async Task<(Stream, string, string)?> GetFileAsync(long id, int? dimension = null)
        {
            if (!_blobFileStorage.CheckIfExist(id, dimension)) return null;

            var stream = _blobFileStorage.OpenBlob(id, dimension);
            var fileRecord = await _blobRepository.GetBlobInfoAsync(id);

            return (stream, fileRecord.FileType, fileRecord.OriginalFileName);

        }

        public async Task DeleteFileAsync(long id, int? dimension = null)
        {
            await _blobRepository.DeleteBlobAsync(id);
            await _blobFileStorage.DeleteBlob(id);
        }
    }
}
