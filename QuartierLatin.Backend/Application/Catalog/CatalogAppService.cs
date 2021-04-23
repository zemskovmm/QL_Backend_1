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

            var response =
                traitTypes.Select(trait => (trait, _commonTraitRepository.GetCommonTraitListByTypeId(trait.Id)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult())).ToList();

            return response;
        }

        public async Task<(int totalPages, List<(University, UniversityLanguage, int cost)>)> GetCatalogPageByFilter(string lang, EntityType entityType,
            Dictionary<CommonTraitType, List<CommonTrait>> commonTraits, int pageNumber, int pageSize)
        {
            var commonTraitsIds = commonTraits.Where(trait => trait.Key.Identifier != "specialty-category" || trait.Key.Identifier != "price")
                .Select(trait => trait.Value.Select(trait => trait.Id).ToList()).ToList();

            var specialtyCategoriesId = new List<int>();
            var priceFiltersId = new List<int>();

            var specialtyCategories = commonTraits
                .FirstOrDefault(specialty => specialty.Key.Identifier == "specialty-category");

            if(specialtyCategories.Value != null)
                specialtyCategoriesId = specialtyCategories.Value.Select(specialty => specialty.Id).ToList();

            var priceFilters = commonTraits
                .FirstOrDefault(price => price.Key.Identifier == "price");

            if (priceFilters.Value != null)
                priceFiltersId = priceFilters.Value
                .Select(price => price.Id).ToList();

            var langId = await _languageRepository.GetLanguageIdByShortNameAsync(lang);

            return await _universityRepository.GetUniversityPageByFilter(commonTraitsIds, specialtyCategoriesId, priceFiltersId, langId, pageSize * pageNumber, pageSize);
        }
    }
}
