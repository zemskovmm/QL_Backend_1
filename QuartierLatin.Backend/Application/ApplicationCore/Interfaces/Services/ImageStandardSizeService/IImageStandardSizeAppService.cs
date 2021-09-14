using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Models.ImageStandardSizeModels;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.ImageStandardSizeService
{
    public interface IImageStandardSizeAppService
    {
        Task<ImageStandardSize> GetImageStandardSizeByIdAsync(int id);
        Task<List<ImageStandardSize>> GetImageStandardSizeListAsync();
        Task<int> CreateImageStandardSizeAsync(int height, int width);
        Task UpdateImageStandardSizeByIdAsync(int id, int height, int width);
        Task DeleteImageStandardSizeByIdAsync(int id);
    }
}