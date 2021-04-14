using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Enums;

namespace QuartierLatin.Backend.Models.Repositories.CatalogRepositoies
{
    public interface ICommonTraitTypeRepository
    {
        Task<int> CreateCommonTraitTypeAsync(Dictionary<string, string> names, string? identifier);
        Task UpdateCommonTraitTypeAsync(int id, Dictionary<string, string> names, string? identifier);
        Task CreateOrUpdateCommonTraitTypesForEntityAsync(int commonTraitId, EntityType entityType);
        Task DeleteCommonTraitTypesForEntityAsync(int commonTraitId, EntityType entityType);
        Task<CommonTraitType> GetCommonTraitTypeAsync(int id);
        Task<List<CommonTraitType>> GetCommonTraitTypeListAsync();
        Task<IEnumerable<int>> GetTraitTypeForEntitiesByEntityTypeIdListAsync(EntityType entityType);
        Task<List<int>> GetEntityTraitToUniversityIdListAsync(int universityId);
        Task CreateEntityTraitToUniversityAsync(int universityId, int commonTraitId);
        Task DeleteEntityTraitToUniversityAsync(int universityId, int commonTraitId);
    }
}
