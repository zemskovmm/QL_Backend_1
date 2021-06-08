using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.Interfaces.Catalog
{
    public interface ICommonTraitTypeAppService
    {
        Task<List<CommonTraitType>> GetTraitTypesAsync();
        Task<int> CreateTraitTypeAsync(string? identifier, Dictionary<string, string> names, int order);
        Task<CommonTraitType> GetTraitTypeByIdAsync(int id);
        Task UpdateTraitTypeByIdAsync(int id, string? identifier, Dictionary<string, string> names, int order);
        Task<IEnumerable<int>> GetTraitTypeForEntitiesByEntityTypeAsync(EntityType entityType);
        Task CreateTraitTypeForEntityByEntityTypeAsync(EntityType entityType, int traitTypeId);
        Task DeleteTraitTypeForEntityByEntityTypeAsync(EntityType entityType, int traitTypeId);
        Task<List<int>> GetEntityTraitToUniversityIdListAsync(int universityId);
        Task CreateEntityTraitToUniversityAsync(int universityId, int commonTraitId);
        Task DeleteEntityTraitToUniversityAsync(int universityId, int commonTraitId);
        Task<List<CommonTraitType>> GetTraitTypesWithIndetifierAsync();
        Task<List<int>> GetEntityTraitToSchoolIdListAsync(int schoolId);
        Task CreateEntityTraitToSchoolAsync(int schoolId, int commonTraitId);
        Task DeleteEntityTraitToSchoolAsync(int schoolId, int commonTraitId);
        Task<List<int>> GetEntityTraitToCurseIdListAsync(int curseId);
        Task CreateEntityTraitToCurseAsync(int curseId, int commonTraitId);
        Task DeleteEntityTraitToCurseAsync(int curseId, int commonTraitId);
    }
}
