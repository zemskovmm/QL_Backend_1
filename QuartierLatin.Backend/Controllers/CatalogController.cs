using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuartierLatin.Backend.Config;
using QuartierLatin.Backend.Dto.CatalogDto;
using QuartierLatin.Backend.Dto;
using QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto;
using QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto.CatalogSearchResponseDto;
using QuartierLatin.Backend.Dto.CommonTraitDto;
using QuartierLatin.Backend.Dto.CourseCatalogDto.Course.CatalogDto;
using QuartierLatin.Backend.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.Catalog;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.CourseCatalog.CourseCatalog;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.CourseCatalog.SchoolCatalog;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.HousingServices;
using QuartierLatin.Backend.Application.ApplicationCore.Models;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Dto.HousingCatalogDto;

namespace QuartierLatin.Backend.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IOptions<BaseFilterOrderConfig> _baseFilterConfig;
        private readonly ICatalogAppService _catalogAppService;
        private readonly ICommonTraitAppService _commonTraitAppService;
        private readonly IDegreeRepository _degreeRepository;
        private readonly ISpecialtyAppService _specialtyAppService;
        private readonly IUniversityGalleryAppService _universityGalleryAppService;
        private readonly ICourseAppService _courseAppService;
        private readonly ICommonTraitTypeAppService _commonTraitTypeAppService;
        private readonly ISchoolAppService _schoolAppService;
        private readonly IHousingAppService _housingAppService;
        private readonly IHousingGalleryAppService _housingGalleryAppService;

        public CatalogController(ICatalogAppService catalogAppService, ISpecialtyAppService specialtyAppService,
            ICommonTraitAppService commonTraitAppService, IDegreeRepository degreeRepository,
            IOptions<BaseFilterOrderConfig> baseFilterConfig, IUniversityGalleryAppService universityGalleryAppService,
            ICourseAppService courseAppService, ICommonTraitTypeAppService commonTraitTypeAppService,
            ISchoolAppService schoolAppService, IHousingAppService housingAppService,
            IHousingGalleryAppService housingGalleryAppService)
        {
            _catalogAppService = catalogAppService;
            _specialtyAppService = specialtyAppService;
            _commonTraitAppService = commonTraitAppService;
            _degreeRepository = degreeRepository;
            _baseFilterConfig = baseFilterConfig;
            _universityGalleryAppService = universityGalleryAppService;
            _courseAppService = courseAppService;
            _commonTraitTypeAppService = commonTraitTypeAppService;
            _schoolAppService = schoolAppService;
            _housingAppService = housingAppService;
            _housingGalleryAppService = housingGalleryAppService;
        }
        
        private string FormatPriceValue(int from, int? to, string lang)
        {
            return lang == "ru"
                ? to is null ? $"От {from} евро" : $"От {from} до {to} евро"
                : lang == "fr"
                    ? to is null ? $"De {from} euros" : $"De {from} à {to} euros"
                    : lang == "esp"
                        ? to is null ? $"De {from} euros" : $"De {from} a {to} euros"
                        : lang == "cn"
                            ? to is null ? $"从 {from} euros" : $"从 {from} 到 {to} euros"
                            : to is null ? $"From {from} euros" : $"From {from} to {to} euros";
        }

        private string FormatPrice(int group, string lang)
        {
            return FormatPriceValue(CostGroup.GetCostGroup(group).from, CostGroup.GetCostGroup(group).to, lang);
        }
		
		
        private string FormatHousingPrice(int group, string lang)
        {
            return FormatPriceValue(CostHousingGroup.GetCostHousingGroup(group).from, CostHousingGroup.GetCostHousingGroup(group).to, lang);
        }		

        // Compatibility with old urls
        [AllowAnonymous]
        [HttpGet("/api/catalog-filters/{lang}/university")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public Task<IActionResult> GetCatalogByLangAndEntityTypeCompat(string lang)
        {
            lang = lang.ToLower();
            return GetCatalogByLangAndEntityType(lang);
        }

        // Compatibility with old urls
        [AllowAnonymous]
        [HttpGet("/api/catalog-filters/{lang}/university/search")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public Task<IActionResult> SearchInCatalogCompat(string lang, [FromBody] CatalogSearchDto catalogSearchDto)
        {
            lang = lang.ToLower();
            return SearchInCatalog(lang, catalogSearchDto);
        }

        [AllowAnonymous]
        [HttpGet("/api/catalog/university/filters/{lang}")]
        public async Task<IActionResult> GetCatalogByLangAndEntityType(string lang)
        {
            lang = lang.ToLower();

            var entityType = EntityType.University;

            var commonTraits = await _catalogAppService.GetNamedCommonTraitsAndTraitTypeByEntityType(entityType);

            var priceLangs = new Dictionary<string, string>
            {
                {"en", "Price"},
                {"ru", "Стоимость"},
                {"fr", "Le coût"},
                {"esp", "El costo"}
            };

            var specLangs = new Dictionary<string, string>
            {
                {"en", "Course"},
                {"ru", "Направление"},
                {"fr", "Les cours"},
                {"esp", "Curso"}
            };

            var degreeLangs = new Dictionary<string, string>
            {
                {"en", "Degree"}, {"ru", "Образование"}, {"esp", "Grado"}, {"fr", "Degré"}
            };

            var degrees = await _degreeRepository.GetAll();
            var specialCategories = await _specialtyAppService.GetSpecialCategoriesList();

            commonTraits.Add((new CommonTraitType
            {
                Identifier = "price",
                Names = priceLangs,
                Order = _baseFilterConfig.Value.PriceOrder
            },
                CostGroup.CostGroups.Select(g =>
                    new CommonTrait
                    {
                        Id = g,
                        Names = new Dictionary<string, string>
                        {
                            [lang] = FormatPrice(g, lang)
                        }
                    }).ToList()));

            commonTraits.Add((new CommonTraitType
            {
                Identifier = "degree",
                Names = degreeLangs,
                Order = _baseFilterConfig.Value.DegreeOrder
            },
                degrees.Select(degree =>
                    new CommonTrait
                    {
                        Id = degree.Id,
                        Names = degree.Names
                    }).ToList()));

            commonTraits.Add((new CommonTraitType
            {
                Identifier = "specialty-category",
                Names = specLangs,
                Order = _baseFilterConfig.Value.CourseOrder
            },
                specialCategories.Select(category =>
                    new CommonTrait
                    {
                        Id = category.Id,
                        Names = category.Names
                    }).ToList()));


            var filters = commonTraits.OrderBy(trait => trait.commonTraitType.Order)
                .Select(trait => new CatalogFilterDto
                {
                    Name = trait.Item1.Names.GetSuitableName(lang),
                    Identifier = trait.Item1.Identifier,
                    Options = trait.Item2.Select(commonTrait => new CatalogOptionsDto
                    {
                        Name = commonTrait.Names.GetSuitableName(lang),
                        Id = commonTrait.Id
                    }).ToList()
                }).ToList();

            var response = new CatalogFilterResponseDto
            {
                Filters = filters
            };

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("/api/catalog/university/search/{lang}")]
        public async Task<IActionResult> SearchInCatalog(string lang, [FromBody] CatalogSearchDto catalogSearchDto)
        {
            lang = lang.ToLower();

            var entityType = EntityType.University;
            var pageSize = catalogSearchDto.PageSize ?? 1000;
            var commonTraits =
                catalogSearchDto.Filters.ToDictionary(filter => filter.Identifier, filter =>
                    filter.Values);

            var catalogPage =
                await _catalogAppService.GetCatalogPageByFilter(lang, entityType, commonTraits,
                    catalogSearchDto.PageNumber, pageSize);

            var universityIds = catalogPage.Item2.Select(x => x.Item1.Id).ToList();
            var traitDic = await _commonTraitAppService.GetTraitsForEntityIds(EntityType.University,
                universityIds);

            var degreeDic = await _degreeRepository.GetDegreesForUniversities(universityIds);

            List<CommonTrait> GetTraits(string identifier, int id)
            {
                if (!traitDic.TryGetValue(id, out var traits))
                    return new List<CommonTrait>();
                return traits.FirstOrDefault(x => x.Key.Identifier == identifier).Value ?? new List<CommonTrait>();
            }

            var universityDtos = new List<CatalogUniversityDto>();

            foreach(var university in catalogPage.Item2)
            {
                var gallery = await _universityGalleryAppService.GetGalleryToUniversityAsync(university.Item1.Id);

                universityDtos.Add(new CatalogUniversityDto
                {
                    Url = $"/{lang}/university/{university.Item2.Url}",
                    LanglessUrl = $"/university/{university.Item2.Url}",
                    Name = university.Item2.Name,
                    PriceFrom = CostGroup.GetCostGroup(university.costGroup).from,
                    PriceTo = CostGroup.GetCostGroup(university.costGroup).to,
                    Degrees = degreeDic.GetValueOrDefault(university.Item2.UniversityId)
                        ?.Select(x => x.Names.GetSuitableName(lang)).ToList() ?? new List<string>(),
                    InstructionLanguages = GetTraits("instruction-language", university.Item1.Id).Select(x => x.Identifier)
                        .ToList(),
                    LogoId = university.Item1.LogoId,
                    BannerId = university.Item1.BannerId,
                    GalleryList = gallery
                });
            }

            var response = new CatalogSearchResponseDtoList<CatalogUniversityDto>
            {
                Items = universityDtos,
                TotalItems = catalogPage.totalItems,
                TotalPages = FilterHelper.PageCount(catalogPage.totalItems, pageSize)
            };

            return Ok(response);
        }
        
        [AllowAnonymous]
        [HttpGet("/api/catalog/course/filters/{lang}")]
        public async Task<IActionResult> GetCatalogFiltersToCourseByLangAndEntityType(string lang)
        {
            lang = lang.ToLower();

            var entityTypeSchool = EntityType.School;
            var entityTypeCourse = EntityType.Course;

            var commonTraitsSchool = await _catalogAppService.GetNamedCommonTraitsAndTraitTypeByEntityType(entityTypeSchool);
            var commonTraitsCourse = await _catalogAppService.GetNamedCommonTraitsAndTraitTypeByEntityType(entityTypeCourse);

            var commonTraits = commonTraitsSchool.Concat(commonTraitsCourse);

            var filters = commonTraits.OrderBy(trait => trait.commonTraitType.Order)
                .Select(trait => new CatalogFilterDto
                {
                    Name = trait.Item1.Names.GetSuitableName(lang),
                    Identifier = trait.Item1.Identifier,
                    Options = trait.Item2.Select(commonTrait => new CatalogOptionsDto
                    {
                        Name = commonTrait.Names.GetSuitableName(lang),
                        Id = commonTrait.Id
                    }).ToList()
                }).ToList();

            var response = new CatalogFilterResponseDto
            {
                Filters = filters
            };

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("/api/catalog/course/search/{lang}")]
        public async Task<IActionResult> SearchInCourseCatalog(string lang, [FromBody] CatalogSearchDto catalogSearchDto)
        {
            lang = lang.ToLower();

            var pageSize = catalogSearchDto.PageSize ?? 1000;

            var commonTraits =
                catalogSearchDto.Filters.ToDictionary(filter => filter.Identifier, filter =>
                    filter.Values);

            var catalogPage =
                await _catalogAppService.GetCatalogCoursePageByFilterAsync(lang, commonTraits,
                    catalogSearchDto.PageNumber, pageSize);

            var courseIds = catalogPage.courseAndLanguage.Select(x => x.course.Id);
            var schoolIds = catalogPage.courseAndLanguage.Select(x => x.course.SchoolId).Distinct();

            var schoolImageIdAndName = await _schoolAppService.GetSchoolImageIdAndNameAndUrlByIdsAsync(schoolIds, lang);

            var commonTraitsCourse = await _courseAppService.GetCommonTraitListByCourseIdsAsync(courseIds);

            var traitsType = await _commonTraitTypeAppService.GetTraitTypesWithIndetifierAsync();

            var courseDtos = new List<CatalogCourseDto>();

            foreach (var course in catalogPage.Item2)
            {
                var traits = new Dictionary<string, List<CommonTraitLanguageDto>>();

                foreach (var traitType in traitsType)
                {
                    var pageTraitTypedList = commonTraitsCourse.GetValueOrDefault(course.course.Id)
                        ?.Where(type => type.CommonTraitTypeId == traitType.Id);

                    if (pageTraitTypedList is null) continue;

                    var courseTraitList = pageTraitTypedList.Select(courseTrait => new CommonTraitLanguageDto
                    {
                        Id = courseTrait.Id,
                        IconBlobId = courseTrait.IconBlobId,
                        Identifier = courseTrait.Identifier,
                        Name = courseTrait.Names.GetSuitableName(lang)
                    }).ToList();

                    traits.Add(traitType.Identifier, courseTraitList);
                }

                courseDtos.Add(new CatalogCourseDto()
                {
                    Url = $"/{lang}/{schoolImageIdAndName[course.course.SchoolId].schoolUrl}/courses/{course.courseLanguage.Url}",
                    LanglessUrl = $"{schoolImageIdAndName[course.course.SchoolId].schoolUrl}/courses/{course.courseLanguage.Url}",
                    Name = course.courseLanguage.Name,
                    CourseImageId = course.course.ImageId,
                    NamedTraits = traits,
                    SchoolImageId = schoolImageIdAndName[course.course.SchoolId].schoolImageId,
                    SchoolName = schoolImageIdAndName[course.course.SchoolId].schoolName,
                    Price = course.course.Price
                });
            };

            var response = new CatalogSearchResponseDtoList<CatalogCourseDto>
            {
                Items = courseDtos,
                TotalItems = catalogPage.totalItems,
                TotalPages = FilterHelper.PageCount(catalogPage.totalItems, pageSize)
            };

            return Ok(response);
        }

        
		
		[AllowAnonymous]
        [HttpGet("/api/catalog/housing/filters/{lang}")]
        public async Task<IActionResult> GetCatalogFiltersTohousingByLangAndEntityType(string lang)
        {

            var priceLangs = new Dictionary<string, string>
            {
                {"en", "Price"},
                {"ru", "Стоимость"},
                {"fr", "Le coût"},
                {"esp", "El costo"}
            };

           var entityType = EntityType.Housing;

            var commonTraits = await _catalogAppService.GetNamedCommonTraitsAndTraitTypeByEntityType(entityType);
                        commonTraits.Add((new CommonTraitType
            {
                Identifier = "price",
                Names = priceLangs,
                Order = _baseFilterConfig.Value.PriceOrder
            },
                CostHousingGroup.CostHousingGroups.Select(g =>
                    new CommonTrait
                    {
                        Id = g,
                        Names = new Dictionary<string, string>
                        {
                            [lang] = FormatHousingPrice(g, lang)
                        }
                    }).ToList()));
	
            var filters = commonTraits.OrderBy(trait => trait.commonTraitType.Order)
                .Select(trait => new CatalogFilterDto
                {
                    Name = trait.Item1.Names.GetSuitableName(lang),
                    Identifier = trait.Item1.Identifier,
                    Options = trait.Item2.Select(commonTrait => new CatalogOptionsDto
                    {
                        Name = commonTrait.Names.GetSuitableName(lang),
                        Id = commonTrait.Id

						
						
						
                    }).OrderBy(o=>o.Name).ToList()
                }).ToList();
				
			 foreach (var curFilter in filters)
             {	
			     if(curFilter.Identifier=="city"){ 
				   curFilter.Options=curFilter.Options;
			       foreach (var curOption in curFilter.Options){
					 var items=await _catalogAppService.GetChildCommonTraitsByParentId(curOption.Id);
			         var childDtoItems=items.Select(commonChildTrait =>  new CatalogOptionsChildDto
                              {
						
				                     Name = commonChildTrait.Names.GetSuitableName(lang),
                                     Id = commonChildTrait.Id	
					          }).ToList();	
			 		curOption.Items=childDtoItems; 
				  }
				 
				 }
			      
			 }

            var response = new CatalogFilterResponseDto
            {
                Filters = filters
            };  

            return Ok(response);
			
			

			
        }
		

        [AllowAnonymous]
        [HttpPost("/api/catalog/housing/search/{lang}")]
        public async Task<IActionResult> SearchInHousingCatalog(string lang, [FromBody] CatalogSearchDto catalogSearchDto)
        {
            lang = lang.ToLower();

            var pageSize = catalogSearchDto.PageSize ?? 1000;

            var commonTraits =
                catalogSearchDto.Filters.ToDictionary(filter => filter.Identifier, filter =>
                    filter.Values);

            var catalogPage =
                await _catalogAppService.GetCatalogHousingPageByFilterAsync(lang, commonTraits,
                    catalogSearchDto.PageNumber, pageSize);

            var housingIds = catalogPage.housingAndLanguage.Select(x => x.housing.Id);

            var commonTraitsHousing = await _housingAppService.GetCommonTraitListByHousingIdsAsync(housingIds);

            var traitsType = await _commonTraitTypeAppService.GetTraitTypesWithIndetifierAsync();

            var housingGallery = await _housingGalleryAppService.GetGalleriesByHousingIdsAsync(housingIds);

            var housingDtos = new List<CatalogHousingDto>();

            foreach (var housing in catalogPage.Item2)
            {
                var traits = new Dictionary<string, List<CommonTraitLanguageDto>>();

                foreach (var traitType in traitsType)
                {
                    var housingTraitTypedList = commonTraitsHousing.GetValueOrDefault(housing.housing.Id)
                        ?.Where(type => type.CommonTraitTypeId == traitType.Id);

                    if (housingTraitTypedList is null) continue;

                    var courseTraitList = housingTraitTypedList.Select(courseTrait => new CommonTraitLanguageDto
                    {
                        Id = courseTrait.Id,
                        IconBlobId = courseTrait.IconBlobId,
                        Identifier = courseTrait.Identifier,
                        Name = courseTrait.Names.GetSuitableName(lang)
                    }).ToList();

                    traits.Add(traitType.Identifier, courseTraitList);
                }

                housingDtos.Add(new CatalogHousingDto()
                {
                    Url = $"/{lang}/housing/{housing.housingLanguage.Url}",
                    LanglessUrl = $"/housing/{housing.housingLanguage.Url}",
                    Name = housing.housingLanguage.Name,
                    ImageId = housing.housing.ImageId,
                    NamedTraits = traits,
                    Price = housing.housing.Price,
                    GalleryList = housingGallery.GetValueOrDefault(housing.housing.Id),
                    Metadata = housing.housingLanguage.Metadata is null ? null : JObject.Parse(housing.housingLanguage.Metadata),
                    Location = housing.housingLanguage.Location is null ? null : JObject.Parse(housing.housingLanguage.Location),
                    HtmlDescription = housing.housingLanguage.Description
                });
            };

            var response = new CatalogSearchResponseDtoList<CatalogHousingDto>
            {
                Items = housingDtos,
                TotalItems = catalogPage.totalItems,
                TotalPages = FilterHelper.PageCount(catalogPage.totalItems, pageSize)
            };

            return Ok(response);
        }
    }
}