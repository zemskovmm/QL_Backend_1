using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.Interfaces.Catalog;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;

namespace QuartierLatin.Backend.Application.Catalog
{
    public class CatalogAppService : ICatalogAppService
    {
        private readonly ICommonTraitRepository _commonTraitRepository;
        private readonly ICommonTraitTypeRepository _commonTraitTypeRepository;

        public CatalogAppService(ICommonTraitRepository commonTraitRepository, ICommonTraitTypeRepository commonTraitTypeRepository)
        {
            _commonTraitTypeRepository = commonTraitTypeRepository;
            _commonTraitRepository = commonTraitRepository;
        }

        public async Task<List<(CommonTraitType, List<CommonTrait>)>> GetNamedCommonTraitsAndTraitTypeByEntityType(EntityType entityType)
        {
            var traitTypes = await _commonTraitTypeRepository.GetTraitTypesWithIndetifierByEntityTypeAsync(entityType);

            var response =
                traitTypes.Select(trait => (trait, _commonTraitRepository.GetCommonTraitListByTypeId(trait.Id)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult())).ToList();

            return response;
        }
    }
}
