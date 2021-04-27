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

        public async Task<(int totalItems, List<(University, UniversityLanguage, int costGroup)>)> GetCatalogPageByFilter(
            string lang, EntityType entityType,
            Dictionary<string, List<int>> commonTraits, int pageNumber, int pageSize)
        {
            var commonTraitIdentifierDic = (await _commonTraitTypeRepository.GetCommonTraitTypeListAsync())
                .ToDictionary(x => string.IsNullOrWhiteSpace(x.Identifier) ? "trait-" + x.Id : x.Identifier);

            var commonTraitsIds = commonTraits
                .Where(trait => trait.Key != "specialty-category" && trait.Key != "price" && trait.Key != "degree")
                .Select(x => x.Value).ToList();

            var specialtyCategoriesId = new List<int>();
            var priceFiltersId = new List<int>();

            var specialtyCategories = commonTraits
                .FirstOrDefault(specialty => specialty.Key == "specialty-category");

            if(specialtyCategories.Value != null)
                specialtyCategoriesId = specialtyCategories.Value.ToList();

            var degrees = commonTraits.FirstOrDefault(x => x.Key == "degree").Value?.ToList() ?? new List<int>();

            var priceFilters = commonTraits
                .FirstOrDefault(price => price.Key == "price");

            if (priceFilters.Value != null)
                priceFiltersId = priceFilters.Value.ToList();

            var langId = await _languageRepository.GetLanguageIdByShortNameAsync(lang);

            return await _universityRepository.GetUniversityPageByFilter(commonTraitsIds, specialtyCategoriesId, degrees,
                priceFiltersId, langId, pageSize * pageNumber, pageSize);
        }
    }
}
