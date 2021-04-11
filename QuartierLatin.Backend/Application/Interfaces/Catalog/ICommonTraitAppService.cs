using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.CatalogModels;

namespace QuartierLatin.Backend.Application.Interfaces.Catalog
{
    public interface ICommonTraitAppService
    {
        Task<List<CommonTrait>> GetTraitOfTypesByTypeIdAsync(int typeId);
        Task<int> CreateCommonTraitAsync(int typeId, JObject names, int order, int? iconBlobId, int? parentId);
        Task<CommonTrait> GetTraitByIdAsync(int id);
        Task UpdateCommonTraitAsync(int id, JObject names, int commonTraitTypeId, int? iconBlobId, int order, int? parentId);
    }
}
