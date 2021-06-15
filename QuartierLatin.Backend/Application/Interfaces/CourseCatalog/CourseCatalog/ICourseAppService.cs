using QuartierLatin.Backend.Models.CourseCatalogModels.CoursesModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.Interfaces.CourseCatalog.CourseCatalog
{
    public interface ICourseAppService
    {
        Task<List<(Course course, Dictionary<int, CourseLanguage> courseLanguage)>> GetCourseListAsync();
        Task<int> CreateCourseAsync(int schoolId);
        Task CreateCourseLanguageListAsync(List<CourseLanguage> courseLanguage);
        Task<(Course course, Dictionary<int, CourseLanguage> schoolLanguage)> GetCourseByIdAsync(int id);
        Task UpdateCourseByIdAsync(int id, int schoolId);
        Task UpdateCourseLanguageByIdAsync(int id, string htmlDescription, int languageId, string name, string url);
    }
}
