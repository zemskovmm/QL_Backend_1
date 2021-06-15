using QuartierLatin.Backend.Models.CourseCatalogModels.CoursesModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Models.Repositories.CourseCatalogRepository.CourseRepository
{
    public interface ICourseCatalogRepository
    {
        Task<List<(Course course, Dictionary<int, CourseLanguage> courseLanguage)>> GetCourseListAsync();
        Task<int> CreateCourseAsync(int schoolId);
        Task CreateCourseLanguageListAsync(List<CourseLanguage> courseLanguage);
        Task<(Course course, Dictionary<int, CourseLanguage> courseLanguage)> GetCourseByIdAsync(int id);
        Task UpdateCourseByIdAsync(int id, int courseId);
        Task CreateOrUpdateCourseLanguageByIdAsync(int id, string htmlDescription, int languageId, string name, string url);
        Task<(Course course, Dictionary<int, CourseLanguage> courseLanguage)> GetCourseByUrlWithLanguageAsync(int languageId, string url);
        Task<(int totalItems, List<(Course course, CourseLanguage courseLanguage)>)> GetCoursePageByFilter(List<List<int>> commonTraitsIds, int langId, int skip, int take);
    }
}
