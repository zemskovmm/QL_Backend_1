using QuartierLatin.Backend.Models.CourseCatalogModels.CoursesModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.CatalogModels;

namespace QuartierLatin.Backend.Application.Interfaces.CourseCatalog.CourseCatalog
{
    public interface ICourseAppService
    {
        Task<List<(Course course, Dictionary<int, CourseLanguage> courseLanguage)>> GetCourseListAsync();
        Task<int> CreateCourseAsync(int schoolId, int? imageId);
        Task CreateCourseLanguageListAsync(List<CourseLanguage> courseLanguage);
        Task<(Course course, Dictionary<int, CourseLanguage> schoolLanguage)> GetCourseByIdAsync(int id);
        Task UpdateCourseByIdAsync(int id, int schoolId, int? imageId);
        Task UpdateCourseLanguageByIdAsync(int id, string htmlDescription, int languageId, string name, string url, JObject? metadata);
        Task<Dictionary<int, List<CommonTrait>>> GetCommonTraitListByCourseIdsAsync(IEnumerable<int> courseIds);
    }
}
