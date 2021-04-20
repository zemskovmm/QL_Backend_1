using QuartierLatin.Backend.Application.Interfaces.Catalog;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Models.Repositories;

namespace QuartierLatin.Backend.Application.Catalog
{
    public class CatalogAppService : ICatalogAppService
    {
        private readonly ICommonTraitRepository _commonTraitRepository;
        private readonly ICommonTraitTypeRepository _commonTraitTypeRepository;
        private readonly IUniversityRepository _universityRepository;
        private readonly ILanguageRepository _languageRepository;

        public CatalogAppService(ICommonTraitRepository commonTraitRepository,
            ICommonTraitTypeRepository commonTraitTypeRepository, IUniversityRepository universityRepository,
            ILanguageRepository languageRepository)
        {
            _commonTraitTypeRepository = commonTraitTypeRepository;
            _commonTraitRepository = commonTraitRepository;
            _universityRepository = universityRepository;
            _languageRepository = languageRepository;
        }

        public async Task<List<(CommonTraitType, List<CommonTrait>)>> GetNamedCommonTraitsAndTraitTypeByEntityType(EntityType entityType)
        {
            var traitTypes = await _commonTraitTypeRepository.GetTraitTypesWithIndetifierByEntityTypeAsync(entityType);

            var traits =
                (await _commonTraitRepository.GetCommonTraitListByTypeIds(traitTypes.Select(x => x.Id).ToArray()))
                .GroupBy(x => x.CommonTraitTypeId).ToDictionary(x => x.Key, x => x.ToList());


            var response =
                traitTypes.Select(trait => (trait, list: traits.GetValueOrDefault(trait.Id))).Where(x => x.list != null)
                    .ToList();

            return response;
        }

        public async Task<(int totalPages, List<(University, UniversityLanguage, int cost)>)> GetCatalogPageByFilter(string lang, EntityType entityType,
            Dictionary<CommonTraitType, List<CommonTrait>> commonTraits, int pageNumber, int pageSize)
        {
            var commonTraitsIds = commonTraits.Values.Select(trait => trait.Select(trait => trait.Id).ToList()).ToList();

            var langId = await _languageRepository.GetLanguageIdByShortNameAsync(lang);

            return await _universityRepository.GetUniversityPageByFilter(pageNumber, commonTraitsIds, langId, pageSize * pageNumber, pageSize);
        }
    }
}
