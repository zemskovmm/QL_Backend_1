using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Models.CatalogModels;

namespace QuartierLatin.Backend.Models.Repositories.CatalogRepositoies
{
    interface ICommonTraitRepository
    {
        Task<int> CreateCommonTraitAsync(int commonTraitTypeId, JObject names, int iconBlobId, int order);
        Task CreateOrUpdateCommonTraitToUniversityAsync(int universityId, int commonTraitId);

        Task UpdateCommonTraitAsync(int id, int commonTraitTypeId, JObject names, int iconBlobId, int order);

        Task DeleteCommonTraitAsync(int id);
        Task DeleteCommonTraitToUniversityAsync(int universityId, int commonTraitId);

        Task<CommonTrait> GetCommonTraitAsync(int id);
    }
}
