using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Models.CatalogModels;

namespace QuartierLatin.Backend.Models.Repositories.CatalogRepositoies
{
    public interface ICommonTraitRepository
    {
        Task<int> CreateCommonTraitAsync(int commonTraitTypeId, JObject names, int? iconBlobId, int order, int? parentId);
        Task CreateOrUpdateCommonTraitToUniversityAsync(int universityId, int commonTraitId);

        Task UpdateCommonTraitAsync(int id, int commonTraitTypeId, JObject names, int? iconBlobId, int order, int? parentId);

        Task DeleteCommonTraitAsync(int id);
        Task DeleteCommonTraitToUniversityAsync(int universityId, int commonTraitId);

        Task<CommonTrait> GetCommonTraitAsync(int id);
        Task<List<CommonTrait>> GetCommonTraitListByTypeId(int typeId);
    }
}
