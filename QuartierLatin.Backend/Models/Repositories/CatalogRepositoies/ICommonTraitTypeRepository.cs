using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Enums;

namespace QuartierLatin.Backend.Models.Repositories.CatalogRepositoies
{
    public interface ICommonTraitTypeRepository
    {
        Task<int> CreateCommonTraitTypeAsync(JObject names, string? identifier);

        Task UpdateCommonTraitTypeAsync(int id, JObject names, string? identifier);
        Task CreateOrUpdateCommonTraitTypesForEntityAsync(int commonTraitId, EntityType entityType);

        Task DeleteCommonTraitTypeAsync(int id);
        Task DeleteCommonTraitTypesForEntityAsync(int commonTraitId);

        Task<CommonTraitType> GetCommonTraitTypeAsync(int id);
        Task<CommonTraitTypesForEntity> GetCommonTraitTypesForEntityAsync(int commonTraitId);
    }
}
