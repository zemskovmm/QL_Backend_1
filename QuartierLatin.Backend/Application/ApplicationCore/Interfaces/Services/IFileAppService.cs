using System.IO;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services
{
    public interface IFileAppService
    {
        Task<int> UploadFileAsync(Stream file, string fileName, string fileType, int? dimension = null, int? id = null, int? storageFolder = null, int? standardSizeId = null);

        Task<(Stream, string, string)?> GetFileAsync(int id, int? dimension = null, int? standardSizeId = null);

        Task<(byte[], string, string)?> GetCompressedFileAsync(int id, int? dimension, int? standardSizeId = null);

        Task DeleteFileAsync(int id);
        Task<int> UploadChatMediaAsync(Stream file, string fileName, string fileType);
    }
}
