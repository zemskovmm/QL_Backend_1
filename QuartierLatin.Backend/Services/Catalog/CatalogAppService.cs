using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CourseCatalogRepository.CourseRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.HousingRepositories;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.Catalog;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CourseCatalogModels.CoursesModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Application.ApplicationCore.Models.HousingModels;

namespace QuartierLatin.Backend.Services.Catalog
{
    public class CatalogAppService : ICatalogAppService
    {
        private readonly ICommonTraitRepository _commonTraitRepository;
        private readonly ICommonTraitTypeRepository _commonTraitTypeRepository;
        private readonly IUniversityRepository _universityRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly ICourseCatalogRepository _courseCatalogRepository;
        private readonly IHousingRepository _housingRepository;

        public CatalogAppService(ICommonTraitRepository commonTraitRepository,
            ICommonTraitTypeRepository commonTraitTypeRepository, IUniversityRepository universityRepository,
            ILanguageRepository languageRepository, ICourseCatalogRepository courseCatalogRepository,
            IHousingRepository housingRepository)
        {
            _commonTraitTypeRepository = commonTraitTypeRepository;
            _commonTraitRepository = commonTraitRepository;
            _universityRepository = universityRepository;
            _languageRepository = languageRepository;
            _courseCatalogRepository = courseCatalogRepository;
            _housingRepository = housingRepository;
        }
		
		
		public async Task<List<CommonTrait>> GetChildCommonTraitsByParentId(int parentId){
            var traits =
                await _commonTraitRepository.GetCommonTraitListByParentId(parentId);
              //  .GroupBy(x => x.CommonTraitTypeId).ToDictionary(x => x.Key, x => x.ToList());	
             return traits;   			  
			
		}

        public async Task<List<(CommonTraitType commonTraitType, List<CommonTrait> commonTraits)>> GetNamedCommonTraitsAndTraitTypeByEntityType(EntityType entityType)
        {
            var traitTypes = await _commonTraitTypeRepository.GetTraitTypesWithIndetifierByEntityTypeAsync(entityType);
			if(entityType==EntityType.Housing){
			  var extra_city_trait_type=await _commonTraitTypeRepository.GetCommonTraitTypeAsync(2);	
              traitTypes.Add(extra_city_trait_type);
		/*	  var extra_acommodation_trait_type=await _commonTraitTypeRepository.GetCommonTraitTypeAsync(22);	
              traitTypes.Add(extra_acommodation_trait_type);*/
			}
            var traits =
                (await _commonTraitRepository.GetCommonTraitListByTypeIds(traitTypes.Select(x => x.Id).ToArray()))
                .GroupBy(x => x.CommonTraitTypeId).ToDictionary(x => x.Key, x => x.ToList());

            var housing_trait_ids =
                await _commonTraitRepository.getHousingTraitIds();
			if(entityType==EntityType.Housing){
              var response =
                traitTypes.Select(trait => (commonTraitType: trait, commonTraits: traits.GetValueOrDefault(trait.Id).Where(ct => housing_trait_ids.Contains(ct.Id) ).OrderBy(ct => ct.Identifier).ToList())).Where(x => x.commonTraits != null)
                    .ToList();
			    return response;		
			}
            else{
            var response =
                traitTypes.Select(trait => (commonTraitType: trait, commonTraits: traits.GetValueOrDefault(trait.Id))).Where(x => x.commonTraits != null)
                    .ToList();
				return response;
       
			}				


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

        public async Task<(int totalItems, List<(Course course, CourseLanguage courseLanguage)> courseAndLanguage)> GetCatalogCoursePageByFilterAsync(string lang, 
            Dictionary<string, List<int>> commonTraits, int pageNumber, int pageSize)
        {
            var commonTraitsIds = commonTraits
                .Select(x => x.Value).ToList();
            var langId = await _languageRepository.GetLanguageIdByShortNameAsync(lang);

            return await _courseCatalogRepository.GetCoursePageByFilter(commonTraitsIds, langId, pageSize * pageNumber, pageSize);
        }

        public async Task<(int totalItems, List<(Housing housing, HousingLanguage housingLanguage)> housingAndLanguage)> GetCatalogHousingPageByFilterAsync(string lang,
            Dictionary<string, List<int>> commonTraits, int pageNumber, int pageSize)
        {
			

      /*      var commonTraitsIds = commonTraits
                .Select(x => x.Value).ToList();*/
			var priceFiltersId = new List<int>();	
            var commonTraitsIds = commonTraits
                .Where(trait => trait.Key != "price")
                .Select(x => x.Value).ToList();		
            var priceFilters = commonTraits
                .FirstOrDefault(price => price.Key == "price");

            if (priceFilters.Value != null)
                priceFiltersId = priceFilters.Value.ToList();				
            var langId = await _languageRepository.GetLanguageIdByShortNameAsync(lang);

            return await _housingRepository.GetHousingPageByFilter(commonTraitsIds, langId, priceFiltersId, pageSize * pageNumber, pageSize);
        }
    }
}
