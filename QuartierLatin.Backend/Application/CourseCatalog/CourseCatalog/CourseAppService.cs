using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.Interfaces.CourseCatalog.CourseCatalog;
using QuartierLatin.Backend.Models.CourseCatalogModels.CoursesModels;
using QuartierLatin.Backend.Models.Repositories.CourseCatalogRepository.CourseRepository;

namespace QuartierLatin.Backend.Application.CourseCatalog.CourseCatalog
{
    public class CourseAppService : ICourseAppService
    {
        private readonly ICourseCatalogRepository _courseCatalogRepository;

        public CourseAppService(ICourseCatalogRepository courseCatalogRepository)
        {
            _courseCatalogRepository = courseCatalogRepository;
        }

        public async Task<List<(Course course, Dictionary<int, CourseLanguage> courseLanguage)>> GetCourseListAsync()
        {
            return await _courseCatalogRepository.GetCourseListAsync();
        }

        public async Task<int> CreateCourseAsync(int schoolId)
        {
            return await _courseCatalogRepository.CreateCourseAsync(schoolId);
        }

        public async Task CreateCourseLanguageListAsync(List<CourseLanguage> courseLanguage)
        {
            await _courseCatalogRepository.CreateCourseLanguageListAsync(courseLanguage);
        }

        public async Task<(Course course, Dictionary<int, CourseLanguage> schoolLanguage)> GetCourseByIdAsync(int id)
        {
            return await _courseCatalogRepository.GetCourseByIdAsync(id);
        }

        public async Task UpdateCourseByIdAsync(int id, int schoolId)
        {
            await _courseCatalogRepository.UpdateCourseByIdAsync(id, schoolId);
        }

        public async Task UpdateCourseLanguageByIdAsync(int id, string htmlDescription, int languageId, string name, string url)
        {
            await _courseCatalogRepository.CreateOrUpdateCourseLanguageByIdAsync(id, htmlDescription, languageId, name, url);
        }
    }
}
