using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CourseCatalogModels.CoursesModels;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.CourseCatalog.CourseCatalog
{
    public interface ICourseAppService
    {
        Task<List<(Course course, Dictionary<int, CourseLanguage> courseLanguage)>> GetCourseListAsync();
        Task<int> CreateCourseAsync(int schoolId, int? imageId, int price, List<CourseLanguage> courseLanguage);
        Task<(Course course, Dictionary<int, CourseLanguage> schoolLanguage)> GetCourseByIdAsync(int id);
        Task UpdateCourseByIdAsync(int id, int schoolId, int? imageId, int price);
        Task UpdateCourseLanguageByIdAsync(int id, string htmlDescription, int languageId, string name, string url, JObject? metadata);
        Task<Dictionary<int, List<CommonTrait>>> GetCommonTraitListByCourseIdsAsync(IEnumerable<int> courseIds);
    }
}
