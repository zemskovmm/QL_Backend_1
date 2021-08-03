using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.CourseCatalogModels.CoursesModels;
using QuartierLatin.Backend.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.Interfaces.Catalog
{
    public interface ICatalogAppService
    {
        Task<List<(CommonTraitType commonTraitType, List<CommonTrait> commonTraits)>> GetNamedCommonTraitsAndTraitTypeByEntityType(EntityType entityType);
        Task<(int totalItems, List<(University, UniversityLanguage, int costGroup)>)> GetCatalogPageByFilter(string lang,
            EntityType entityType, Dictionary<string, List<int>> commonTraits, int pageNumber, int pageSize);
        Task<(int totalItems, List<(Course course, CourseLanguage courseLanguage)>)> GetCatalogCoursePageByFilterAsync(string lang, 
            Dictionary<string, List<int>> commonTraits, int pageNumber, int pageSize);
    }
}
