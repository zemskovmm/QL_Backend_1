using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Storages;
using QuartierLatin.Backend.Utils;
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
        public async Task<int> UploadFileAsync(Stream file, string fileName, string fileType, int? dimension = null, int? id = null, int? storageFolder = null)
        {
            if (dimension is null && id is null)
            {
                var newId = await _blobRepository.CreateBlobIdAsync(fileType, fileName, storageFolder);
                await _blobFileStorage.CreateBlobAsync(newId, file);

                return newId;
            }
            else
            {
                await _blobFileStorage.CreateBlobAsync(id.Value, file, dimension);
                return id.Value;
            }
           
        }

        public async Task<(Stream, string, string)?> GetFileAsync(int id, int? dimension = null)
        {
            var fileRecord = await _blobRepository.GetBlobInfoAsync(id);

            if (fileRecord.FileType == "image/svg+xml")
            {
                var streamNotScaled = _blobFileStorage.OpenBlob(id);

                return (streamNotScaled, fileRecord.FileType, fileRecord.OriginalFileName);
            }

            if (!_blobFileStorage.CheckIfExist(id, dimension)) return null;

            var stream = _blobFileStorage.OpenBlob(id, dimension);

            return (stream, fileRecord.FileType, fileRecord.OriginalFileName);

        }

        public async Task<(byte[], string, string)?> GetCompressedFileAsync(int id, int dimension)
        {
            await using var stream = new MemoryStream();

            var responseFromService = await GetFileAsync(id, dimension);

            if (responseFromService is null)
            {
                responseFromService = await GetFileAsync(id);

                var imageScaler = new ImageScaler(dimension);

                imageScaler.Scale(responseFromService.Value.Item1, stream);

                var fileContent = stream.ToArray();

                await using var fileStream = new MemoryStream(fileContent);

                await UploadFileAsync(fileStream, responseFromService.Value.Item3, responseFromService.Value.Item2, dimension, id);

                return (fileStream.ToArray(), responseFromService.Value.Item2, responseFromService.Value.Item3);
            }
            else
            {
                await responseFromService.Value.Item1.CopyToAsync(stream);

                return (stream.ToArray(), responseFromService.Value.Item2, responseFromService.Value.Item3);
            }
        }

        public async Task DeleteFileAsync(int id, int? dimension = null)
        {
            await _blobRepository.DeleteBlobAsync(id);
            await _blobFileStorage.DeleteBlob(id);
        }
    }
}
