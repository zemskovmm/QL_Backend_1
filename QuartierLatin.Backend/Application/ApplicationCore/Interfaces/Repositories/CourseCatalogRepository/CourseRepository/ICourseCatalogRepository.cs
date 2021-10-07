using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CourseCatalogModels.CoursesModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CourseCatalogModels.SchoolModels;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CourseCatalogRepository.CourseRepository
{
    public interface ICourseCatalogRepository
    {
        Task<List<(Course course, Dictionary<int, CourseLanguage> courseLanguage)>> GetCourseListAsync();
        Task<int> CreateCourseAsync(int schoolId, int? imageId, int price, List<CourseLanguage> courseLanguage);
        Task<(Course course, Dictionary<int, CourseLanguage> courseLanguage)> GetCourseByIdAsync(int id);
        Task UpdateCourseByIdAsync(int id, int courseId, int? imageId, int price);
        Task CreateOrUpdateCourseLanguageByIdAsync(int id, string htmlDescription, int languageId, string name, string url, JObject? metadata);
        Task<(Course course, Dictionary<int, CourseLanguage> courseLanguage)> GetCourseByUrlWithLanguageAsync(int languageId, string url);
        Task<(int totalItems, List<(Course course, CourseLanguage courseLanguage)>)> GetCoursePageByFilter(List<List<int>> commonTraitsIds, int langId, int skip, int take);
        Task<List<string>> GetCourseUrlsListAsync(int schoolId);
        Task<List<(Course course, Dictionary<int, CourseLanguage> courseLanguage, Dictionary<int, SchoolLanguages> schoolLanguage)>> GetCoursesListAsync(int schoolId);
    }
}
