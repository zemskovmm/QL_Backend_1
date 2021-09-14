using System.IO;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.ImageStandardSizeService;
using QuartierLatin.Backend.Storages;
using QuartierLatin.Backend.Utils;

namespace QuartierLatin.Backend.Services
{
    public class FileAppService : IFileAppService
    {
        private readonly IBlobFileStorage _blobFileStorage;
        private readonly IBlobRepository _blobRepository;
        private readonly IImageStandardSizeAppService _imageStandardSizeAppService;
        public FileAppService(IBlobFileStorage blobFileStorage, IBlobRepository blobRepository, IImageStandardSizeAppService imageStandardSizeAppService)
        {
            _blobFileStorage = blobFileStorage;
            _blobRepository = blobRepository;
            _imageStandardSizeAppService = imageStandardSizeAppService;
        }
        public async Task<int> UploadFileAsync(Stream file, string fileName, string fileType, int? dimension = null, int? id = null, int? storageFolder = null, int? standardSizeId = null)
        {
            if (dimension is null && id is null && !standardSizeId.HasValue)
            {
                var newId = await _blobRepository.CreateBlobIdAsync(fileType, fileName, storageFolder);
                await _blobFileStorage.CreateBlobAsync(newId, file);

                return newId;
            }
            else
            {
                if (standardSizeId.HasValue)
                {
                    var standardImageSize = await
                        _imageStandardSizeAppService.GetImageStandardSizeByIdAsync(standardSizeId.Value);

                    await _blobFileStorage.CreateBlobAsync(id.Value, file, dimension, standardImageSize.Width, standardImageSize.Height);
                }
                else
                    await _blobFileStorage.CreateBlobAsync(id.Value, file, dimension);

                return id.Value;
            }
           
        }

        public async Task<(Stream, string, string)?> GetFileAsync(int id, int? dimension = null, int? standardSizeId = null)
        {
            var fileRecord = await _blobRepository.GetBlobInfoAsync(id);

            if (!_blobFileStorage.CheckIfExist(id, dimension)) return null;

            if (fileRecord.FileType == "image/svg+xml")
            {
                var streamNotScaled = _blobFileStorage.OpenBlob(id);
                return (streamNotScaled, fileRecord.FileType, fileRecord.OriginalFileName);
            }

            if (!standardSizeId.HasValue)
            {
                if (!_blobFileStorage.CheckIfExist(id, dimension))
                {
                    var compressedFile = await CompressAndUploadFile(id, dimension, standardSizeId);

                    return (new MemoryStream(compressedFile.Value.Item1), compressedFile.Value.Item2, compressedFile.Value.Item3);
                }
                var stream = _blobFileStorage.OpenBlob(id, dimension);

                return (stream, fileRecord.FileType, fileRecord.OriginalFileName);
            }

            var standardImageSize = await 
                _imageStandardSizeAppService.GetImageStandardSizeByIdAsync(standardSizeId.Value);

            if (!_blobFileStorage.CheckIfExist(id, dimension, standardImageSize.Width, standardImageSize.Height))
            {
                var compressedFile = await CompressAndUploadFile(id, dimension, standardSizeId);

                return (new MemoryStream(compressedFile.Value.Item1), compressedFile.Value.Item2, compressedFile.Value.Item3);
            }

            var streamStandardSize = _blobFileStorage.OpenBlob(id, dimension, standardImageSize.Width, standardImageSize.Height);
            var fileRecordStandardSize = await _blobRepository.GetBlobInfoAsync(id);

            return (streamStandardSize, fileRecordStandardSize.FileType, fileRecordStandardSize.OriginalFileName);

        }

        public async Task<(byte[], string, string)?> GetCompressedFileAsync(int id, int? dimension, int? standardSizeId)
        {
            var responseFromService = await GetFileAsync(id, dimension, standardSizeId);

            if (responseFromService is null)
            {
                return await CompressAndUploadFile(id, dimension, standardSizeId);
            }
            else
            {
                await using var stream = new MemoryStream();

                await responseFromService.Value.Item1.CopyToAsync(stream);

                return (stream.ToArray(), responseFromService.Value.Item2, responseFromService.Value.Item3);
            }
        }

        public async Task DeleteFileAsync(int id)
        {
            await _blobRepository.DeleteBlobAsync(id);
        }

        private async Task<(byte[], string, string)?> CompressAndUploadFile(int id, int? dimension, int? standardSizeId)
        {
            await using var stream = new MemoryStream();

            var file = await GetFileAsync(id);

            var imageScaler = dimension.HasValue ? new ImageScaler(dimension.Value) : new ImageScaler();

            if (standardSizeId.HasValue)
            {
                var standardImageSize = await
                    _imageStandardSizeAppService.GetImageStandardSizeByIdAsync(standardSizeId.Value);

                imageScaler.Scale(file.Value.Item1, stream, width: standardImageSize.Width, height: standardImageSize.Height);
            }
            else
            {
                imageScaler.Scale(file.Value.Item1, stream);
            }

            var fileContent = stream.ToArray();

            await using var fileStream = new MemoryStream(fileContent);

            await UploadFileAsync(fileStream, file.Value.Item3, file.Value.Item2, dimension, id, standardSizeId: standardSizeId);

            return (fileStream.ToArray(), file.Value.Item2, file.Value.Item3);
        }
    }
}
