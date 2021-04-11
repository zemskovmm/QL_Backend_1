using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.Interfaces.Catalog;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;

namespace QuartierLatin.Backend.Application.Catalog
{
    public class CommonTraitAppService : ICommonTraitAppService
    {
        private readonly ICommonTraitRepository _commonTraitRepository;

        public CommonTraitAppService(ICommonTraitRepository commonTraitRepository)
        {
            _commonTraitRepository = commonTraitRepository;
        }

        public async Task<List<CommonTrait>> GetTraitOfTypesByTypeIdAsync(int typeId)
        {
            return await _commonTraitRepository.GetCommonTraitListByTypeId(typeId);
        }

        public async Task<int> CreateCommonTraitAsync(int typeId, JObject names, int order, int? iconBlobId, int? parentId)
        {
            return await _commonTraitRepository.CreateCommonTraitAsync(typeId, names, iconBlobId, order, parentId);
        }

        public async Task<CommonTrait> GetTraitByIdAsync(int id)
        {
            return await _commonTraitRepository.GetCommonTraitAsync(id);
        }

        public async Task UpdateCommonTraitAsync(int id, JObject names, int commonTraitTypeId, int? iconBlobId,
            int order, int? parentId)
        {
            await _commonTraitRepository.UpdateCommonTraitAsync(id, commonTraitTypeId, names, iconBlobId, order, parentId);
        }
    }
}