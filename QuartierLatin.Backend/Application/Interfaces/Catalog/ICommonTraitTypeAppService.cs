using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.Interfaces.Catalog
{
    public interface ICommonTraitTypeAppService
    {
        Task<List<CommonTraitType>> GetTraitTypesAsync();
        Task<int> CreateTraitTypeAsync(string? identifier, Dictionary<string, string> names);
        Task<CommonTraitType> GetTraitTypeByIdAsync(int id);
        Task UpdateTraitTypeByIdAsync(int id, string? identifier, Dictionary<string, string> names);
        Task<IEnumerable<int>> GetTraitTypeForEntitiesByEntityTypeAsync(EntityType entityType);
        Task CreateTraitTypeForEntityByEntityTypeAsync(EntityType entityType, int traitTypeId);
        Task DeleteTraitTypeForEntityByEntityTypeAsync(EntityType entityType, int traitTypeId);
        Task<List<int>> GetEntityTraitToUniversityIdListAsync(int universityId);
        Task CreateEntityTraitToUniversityAsync(int universityId, int commonTraitId);
        Task DeleteEntityTraitToUniversityAsync(int universityId, int commonTraitId);
    }
}
