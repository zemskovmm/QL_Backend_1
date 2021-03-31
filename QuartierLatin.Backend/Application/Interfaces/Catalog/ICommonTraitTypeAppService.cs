using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Enums;

namespace QuartierLatin.Backend.Application.Interfaces.Catalog
{
    public interface ICommonTraitTypeAppService
    {
        Task<List<CommonTraitType>> GetTraitTypesAsync();
        Task<int> CreateTraitTypeAsync(string? identifier, JObject names);
        Task<CommonTraitType> GetTraitTypeByIdAsync(int id);
        Task UpdateTraitTypeByIdAsync(int id, string? identifier, JObject names);
        Task<IEnumerable<int>> GetTraitTypeForEntitiesByEntityTypeAsync(EntityType entityType);
        Task CreateTraitTypeForEntityByEntityTypeAsync(EntityType entityType, int traitTypeId);
        Task DeleteTraitTypeForEntityByEntityTypeAsync(EntityType entityType, int traitTypeId);
        Task<List<int>> GetEntityTraitToUniversityIdListAsync(int universityId);
        Task CreateEntityTraitToUniversityAsync(int universityId, int commonTraitId);
        Task DeleteEntityTraitToUniversityAsync(int universityId, int commonTraitId);
    }
}
