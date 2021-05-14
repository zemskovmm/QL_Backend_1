using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.Interfaces
{
    public interface IFileAppService
    {
        Task<int> UploadFileAsync(Stream file, string fileName, string fileType, int? dimension = null, int? id = null, int? storageFolder = null);

        Task<(Stream, string, string)?> GetFileAsync(int id, int? dimension = null);

        Task<(byte[], string, string)?> GetCompressedFileAsync(int id, int dimension);

        Task DeleteFileAsync(int id, int? dimension = null);
    }
}
