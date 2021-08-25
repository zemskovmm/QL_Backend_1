﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.Interfaces.CourseCatalog.CourseCatalog
{
    public interface ICourseGalleryAppService
    {
        Task CreateGalleryItemToCourseAsync(int courseId, int imageId);
        Task DeleteGalleryItemToCourseAsync(int courseId, int imageId);
        Task<List<int>> GetGalleryToCourseAsync(int courseId);
    }
}
