using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CourseCatalogModels.CoursesModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Application.ApplicationCore.Models.HousingModels;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.Catalog
{
    public interface ICatalogAppService
    {
        Task<List<(CommonTraitType commonTraitType, List<CommonTrait> commonTraits)>> GetNamedCommonTraitsAndTraitTypeByEntityType(EntityType entityType);
        Task<(int totalItems, List<(University, UniversityLanguage, int costGroup)>)> GetCatalogPageByFilter(string lang,
            EntityType entityType, Dictionary<string, List<int>> commonTraits, int pageNumber, int pageSize);
        Task<(int totalItems, List<(Course course, CourseLanguage courseLanguage)> courseAndLanguage)> GetCatalogCoursePageByFilterAsync(string lang, 
            Dictionary<string, List<int>> commonTraits, int pageNumber, int pageSize);
        Task<(int totalItems, List<(Housing housing, HousingLanguage housingLanguage)> housingAndLanguage)> GetCatalogHousingPageByFilterAsync(string lang,
            Dictionary<string, List<int>> commonTraits, int pageNumber, int pageSize);
    }
}
