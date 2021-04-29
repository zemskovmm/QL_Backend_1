using QuartierLatin.Backend.Models.CatalogModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Models.Enums;

namespace QuartierLatin.Backend.Application.Interfaces.Catalog
{
    public interface ICatalogAppService
    {
        Task<List<(CommonTraitType commonTraitType, List<CommonTrait> commonTraits)>> GetNamedCommonTraitsAndTraitTypeByEntityType(EntityType entityType);
        Task<(int totalPages, List<(University university, UniversityLanguage universityLanguage, int cost)> universities)> GetCatalogPageByFilter(string lang,
            EntityType entityType, Dictionary<CommonTraitType, List<CommonTrait>> commonTraits, int pageNumber, int pageSize);
    }
}
