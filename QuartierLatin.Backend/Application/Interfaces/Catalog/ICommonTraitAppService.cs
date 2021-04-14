using QuartierLatin.Backend.Models.CatalogModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.Interfaces.Catalog
{
    public interface ICommonTraitAppService
    {
        Task<List<CommonTrait>> GetTraitOfTypesByTypeIdAsync(int typeId);
        Task<int> CreateCommonTraitAsync(int typeId, Dictionary<string, string> names, int order, int? iconBlobId, int? parentId);
        Task<CommonTrait> GetTraitByIdAsync(int id);
        Task UpdateCommonTraitAsync(int id, Dictionary<string, string> names, int commonTraitTypeId, int? iconBlobId, int order, int? parentId);
        Task<List<CommonTrait>> GetTraitOfTypesByTypeIdAndUniversityIdAsync(int typeId, int universityId);
    }
}
