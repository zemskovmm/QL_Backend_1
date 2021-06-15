using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Application.Interfaces.Catalog;
using QuartierLatin.Backend.Dto.CatalogDto;
using QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto;
using QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto.CatalogSearchResponseDto;
using QuartierLatin.Backend.Dto.CourseCatalogDto.Course.CatalogDto;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogAppService _catalogAppService;
        private readonly ISpecialtyAppService _specialtyAppService;
        private readonly ICommonTraitAppService _commonTraitAppService;
        private readonly IDegreeRepository _degreeRepository;

        public CatalogController(ICatalogAppService catalogAppService, ISpecialtyAppService specialtyAppService, ICommonTraitAppService commonTraitAppService, IDegreeRepository degreeRepository)
        {
            _catalogAppService = catalogAppService;
            _specialtyAppService = specialtyAppService;
            _commonTraitAppService = commonTraitAppService;
            _degreeRepository = degreeRepository;
        }

        // Compatibility with old urls
        [AllowAnonymous]
        [HttpGet("/api/catalog-filters/{lang}/university")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public Task<IActionResult> GetCatalogByLangAndEntityTypeCompat(string lang) =>
            GetCatalogByLangAndEntityType(lang);
        
        // Compatibility with old urls
        [AllowAnonymous]
        [HttpGet("/api/catalog-filters/{lang}/university/search")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public Task<IActionResult> SearchInCatalogCompat(string lang, [FromBody] CatalogSearchDto catalogSearchDto) =>
            SearchInCatalog(lang, catalogSearchDto);
        
        [AllowAnonymous]
        [HttpGet("/api/catalog/university/filters/{lang}")]
        public async Task<IActionResult> GetCatalogByLangAndEntityType(string lang)
        {
            var entityType = EntityType.University;

            var commonTraits = await _catalogAppService.GetNamedCommonTraitsAndTraitTypeByEntityType(entityType);

            /*
            var priceLangVersion = new Dictionary<string, Func<int, (string title, List<)>
            {
                {"en", ("Price", "Up to 10000 euros", "Up to 20000 euros", "Up to 30000 euros", "Course")},
                {"ru", ("Стоимость", "До 10000 евро", "До 20000 евро", "До 30000 евро", "Направление")},
                {"fr", ("Le coût", "Jusqu'à 10000 euros", "Jusqu'à 20000 euros", "Jusqu'à 30000 euros", "Les cours")}, 
                {"esp", ("El costo", "Hasta 10000 euros", "Hasta 20000 euros", "Hasta 30000 euros", "Curso")}
            };*/

            var priceLangs = new Dictionary<string, string>()
            {
                {"en", "Price"},
                {"ru", "Стоимость"},
                {"fr", "Le coût"},
                {"esp", "El costo"}
            };
            
            var specLangs = new Dictionary<string, string>()
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

            string FormatPriceValue(int price) => lang == "ru"
                ? $"До {price} евро"
                : lang == "fr"
                    ? $"Jusqu'à {price} euros"
                    : lang == "esp"
                        ? $"Hasta {price} euros"
                        : $"Up to {price} euros";

            string FormatPrice(int group) => FormatPriceValue(CostGroup.GetCostGroup(group).to);

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

            filters.Add(new CatalogFilterDto
            {
                Identifier = "price",
                Name = priceLangs.GetSuitableName(lang),
                Options = CostGroup.CostGroups.Select(g =>
                    new CatalogOptionsDto
                    {
                        Id = g,
                        Name = FormatPrice(g)
                    }).ToList()
            });

            var degrees = await _degreeRepository.GetAll();
            filters.Add(new CatalogFilterDto
            {
                Identifier = "degree",
                Name = degreeLangs.GetSuitableName(lang),
                Options = degrees.Select(degree =>
                    new CatalogOptionsDto
                    {
                        Id = degree.Id,
                        Name = degree.Names.GetSuitableName(lang)
                    }).ToList()
            });


            var specialCategories = await _specialtyAppService.GetSpecialCategoriesList();

            filters.Add(new CatalogFilterDto
            {
                Identifier = "specialty-category",
                Name = specLangs.GetSuitableName(lang),
                Options = specialCategories.Select(category => new CatalogOptionsDto
                {
                    Id = category.Id,
                    Name = category.Names.GetSuitableName(lang)
                }).ToList()
            });
                 
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

            var universityDtos = catalogPage.Item2.Select(university => new CatalogUniversityDto()
            {
                Url = $"/{lang}/university/{university.Item2.Url}",
                LanglessUrl = $"/university/{university.Item2.Url}",
                Name = university.Item2.Name,
                PriceFrom = CostGroup.GetCostGroup(university.costGroup).from,
                PriceTo = CostGroup.GetCostGroup(university.costGroup).to,
                Degrees = degreeDic.GetValueOrDefault(university.Item2.UniversityId)
                    ?.Select(x => x.Names.GetSuitableName(lang)).ToList() ?? new List<string>(),
                InstructionLanguages = GetTraits("instruction-language", university.Item1.Id).Select(x => x.Identifier)
                    .ToList()
            }).ToList();

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
        public async Task<IActionResult> GetCatalogFiltersTocourseByLangAndEntityType(string lang)
        {
            var entityType = EntityType.School;

            var commonTraits = await _catalogAppService.GetNamedCommonTraitsAndTraitTypeByEntityType(entityType);

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
            var entityType = EntityType.University;
            var pageSize = catalogSearchDto.PageSize ?? 1000;
            var commonTraits =
                catalogSearchDto.Filters.ToDictionary(filter => filter.Identifier, filter =>
                    filter.Values);

            var catalogPage =
                await _catalogAppService.GetCatalogCoursePageByFilterAsync(lang, entityType, commonTraits,
                    catalogSearchDto.PageNumber, pageSize);

            var courseIds = catalogPage.Item2.Select(x => x.Item1.Id).ToList();
            var traitDic = await _commonTraitAppService.GetTraitsForEntityIds(EntityType.University,
                courseIds);

            List<CommonTrait> GetTraits(string identifier, int id)
            {
                if (!traitDic.TryGetValue(id, out var traits))
                    return new List<CommonTrait>();
                return traits.FirstOrDefault(x => x.Key.Identifier == identifier).Value ?? new List<CommonTrait>();
            }

            var courseDtos = catalogPage.Item2.Select(course => new CatalogCourseDto()
            {
                Url = $"/{lang}/course/{course.Item2.Url}",
                LanglessUrl = $"/course/{course.Item2.Url}",
                Name = course.Item2.Name,
                
            }).ToList();

            var response = new CatalogSearchResponseDtoList<CatalogCourseDto>
            {
                Items = courseDtos,
                TotalItems = catalogPage.totalItems,
                TotalPages = FilterHelper.PageCount(catalogPage.totalItems, pageSize)
            };

            return Ok(response);
        }
    }
}
