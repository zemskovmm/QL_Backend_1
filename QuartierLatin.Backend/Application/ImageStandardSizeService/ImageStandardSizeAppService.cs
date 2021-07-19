using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.Interfaces.ImageStandardSizeService;
using QuartierLatin.Backend.Models.ImageStandardSizeModels;
using QuartierLatin.Backend.Models.Repositories.ImageRepository;

namespace QuartierLatin.Backend.Application.ImageStandardSizeService
{
    public class ImageStandardSizeAppService : IImageStandardSizeAppService
    {
        private readonly IImageStandardSizeRepository _imageStandardSizeRepository;
        public ImageStandardSizeAppService(IImageStandardSizeRepository imageStandardSizeRepository)
        {
            _imageStandardSizeRepository = imageStandardSizeRepository;
        }

        public async Task<ImageStandardSize> GetImageStandardSizeByIdAsync(int id)
        {
            return await _imageStandardSizeRepository.GetImageStandardSizeByIdAsync(id);
        }

        public async Task<List<ImageStandardSize>> GetImageStandardSizeListAsync()
        {
            return await _imageStandardSizeRepository.GetImageStandardSizeListAsync();
        }

        public async Task<int> CreateImageStandardSizeAsync(int height, int width)
        {
            return await _imageStandardSizeRepository.CreateImageStandardSizeAsync(height, width);
        }

        public async Task UpdateImageStandardSizeByIdAsync(int id, int height, int width)
        {
            await _imageStandardSizeRepository.UpdateImageStandardSizeByIdAsync(id, height, width);
        }

        public async Task DeleteImageStandardSizeByIdAsync(int id)
        {
            await _imageStandardSizeRepository.DeleteImageStandardSizeByIdAsync(id);
        }
    }
}
