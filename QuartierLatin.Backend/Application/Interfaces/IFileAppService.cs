using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.Interfaces
{
    public interface IFileAppService
    {
        Task<long> UploadFileAsync(Stream file, string fileName, string fileType, int? dimension = null, long ? id = null);

        Task<(Stream, string, string)?> GetFileAsync(long id, int? dimension = null);

        Task DeleteFileAsync(long id, int? dimension = null);
    }
}
