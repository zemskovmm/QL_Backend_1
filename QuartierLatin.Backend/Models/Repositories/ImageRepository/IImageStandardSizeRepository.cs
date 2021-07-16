﻿using QuartierLatin.Backend.Models.ImageStandardSizeModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Models.Repositories.ImageRepository
{
    public interface IImageStandardSizeRepository
    {
        Task<ImageStandardSize> GetImageStandardSizeByIdAsync(int id);
        Task<List<ImageStandardSize>> GetImageStandardSizeListAsync();
        Task<int> CreateImageStandardSizeAsync(int height, int width);
        Task UpdateImageStandardSizeByIdAsync(int id, int height, int width);
        Task DeleteImageStandardSizeByIdAsync(int id);
    }
}
