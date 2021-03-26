using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.Interfaces
{
    public interface IFileAppService
    {
        Task<long> UploadFileAsync(IFormFile file, string fileType);

        Task<(Stream, string, string)> GetFileAsync(long id);

        Task DeleteFileAsync(long id);
    }
}
