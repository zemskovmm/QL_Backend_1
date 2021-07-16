using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Models.Repositories.CatalogRepositoies
{
    public interface ICommonTraitTypeRepository
    {
        Task<int> CreateCommonTraitTypeAsync(Dictionary<string, string> names, string? identifier, int order);
        Task UpdateCommonTraitTypeAsync(int id, Dictionary<string, string> names, string? identifier, int order);
        Task CreateOrUpdateCommonTraitTypesForEntityAsync(int commonTraitId, EntityType entityType);
        Task DeleteCommonTraitTypesForEntityAsync(int commonTraitId, EntityType entityType);
        Task<CommonTraitType> GetCommonTraitTypeAsync(int id);
        Task<List<CommonTraitType>> GetCommonTraitTypeListAsync();
        Task<IEnumerable<int>> GetTraitTypeForEntitiesByEntityTypeIdListAsync(EntityType entityType);
        Task<List<int>> GetEntityTraitToUniversityIdListAsync(int universityId);
        Task CreateEntityTraitToUniversityAsync(int universityId, int commonTraitId);
        Task DeleteEntityTraitToUniversityAsync(int universityId, int commonTraitId);
        Task<List<CommonTraitType>> GetTraitTypesWithIndetifierAsync();
        Task<List<CommonTraitType>> GetTraitTypesWithIndetifierByEntityTypeAsync(EntityType entityType);
        Task<List<int>> GetEntityTraitToSchoolIdListAsync(int schoolId);
        Task CreateEntityTraitToSchoolAsync(int schoolId, int commonTraitId);
        Task DeleteEntityTraitToSchoolAsync(int schoolId, int commonTraitId);
        Task<List<int>> GetEntityTraitToCourseIdListAsync(int courseId);
        Task CreateEntityTraitToCourseAsync(int courseId, int commonTraitId);
        Task DeleteEntityTraitToCourseAsync(int courseId, int commonTraitId);
        Task<List<int>> GetEntityTraitToUniversityIdByCommonTraitTypeIdListAsync(int universityId, int commonTraitTypeId);
        Task<List<int>> GetEntityTraitToSchoolIdByCommonTraitTypeIdListAsync(int schoolId, int commonTraitTypeId);
        Task<List<int>> GetEntityTraitToCourseIdByCommonTraitTypeIdListAsync(int courseId, int commonTraitTypeId);
        Task<List<int>> GetEntityTraitToPageIdListAsync(int pageId);
        Task CreateEntityTraitToPageAsync(int pageId, int commonTraitId);
        Task DeleteEntityTraitToPageAsync(int pageId, int commonTraitId);
    }
}
