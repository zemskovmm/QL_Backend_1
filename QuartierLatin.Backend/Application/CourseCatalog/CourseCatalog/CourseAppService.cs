using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.Interfaces.CourseCatalog.CourseCatalog;
using QuartierLatin.Backend.Models.CourseCatalogModels.CoursesModels;
using QuartierLatin.Backend.Models.Repositories.AppStateRepository;
using QuartierLatin.Backend.Models.Repositories.CourseCatalogRepository.CourseRepository;

namespace QuartierLatin.Backend.Application.CourseCatalog.CourseCatalog
{
    public class CourseAppService : ICourseAppService
    {
        private readonly ICourseCatalogRepository _courseCatalogRepository;
        private readonly IAppStateEntryRepository _appStateEntryRepository;

        public CourseAppService(ICourseCatalogRepository courseCatalogRepository, IAppStateEntryRepository appStateEntryRepository)
        {
            _courseCatalogRepository = courseCatalogRepository;
            _appStateEntryRepository = appStateEntryRepository;
        }

        public async Task<List<(Course course, Dictionary<int, CourseLanguage> courseLanguage)>> GetCourseListAsync()
        {
            return await _courseCatalogRepository.GetCourseListAsync();
        }

        public async Task<int> CreateCourseAsync(int schoolId)
        {
            var id = await _courseCatalogRepository.CreateCourseAsync(schoolId);
            await _appStateEntryRepository.UpdateLastChangeTimeAsync();
            return id;
        }

        public async Task CreateCourseLanguageListAsync(List<CourseLanguage> courseLanguage)
        {
            await _courseCatalogRepository.CreateCourseLanguageListAsync(courseLanguage);
            await _appStateEntryRepository.UpdateLastChangeTimeAsync();
        }

        public async Task<(Course course, Dictionary<int, CourseLanguage> schoolLanguage)> GetCourseByIdAsync(int id)
        {
            return await _courseCatalogRepository.GetCourseByIdAsync(id);
        }

        public async Task UpdateCourseByIdAsync(int id, int schoolId)
        {
            await _courseCatalogRepository.UpdateCourseByIdAsync(id, schoolId);
            await _appStateEntryRepository.UpdateLastChangeTimeAsync();
        }

        public async Task UpdateCourseLanguageByIdAsync(int id, string htmlDescription, int languageId, string name, string url, JObject? metadata)
        {
            await _courseCatalogRepository.CreateOrUpdateCourseLanguageByIdAsync(id, htmlDescription, languageId, name, url, metadata);
            await _appStateEntryRepository.UpdateLastChangeTimeAsync();
        }
    }
}
