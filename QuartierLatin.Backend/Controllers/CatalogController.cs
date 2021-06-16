using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Application.Interfaces.Catalog;
using QuartierLatin.Backend.Config;
using QuartierLatin.Backend.Dto.CatalogDto;
using QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto;
using QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto.CatalogSearchResponseDto;
using QuartierLatin.Backend.Models;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Utils;

namespace QuartierLatin.Backend.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IOptions<BaseFilterOrderConfig> _baseFilterConfig;
        private readonly ICatalogAppService _catalogAppService;
        private readonly ICommonTraitAppService _commonTraitAppService;
        private readonly IDegreeRepository _degreeRepository;
        private readonly ISpecialtyAppService _specialtyAppService;

        public CatalogController(ICatalogAppService catalogAppService, ISpecialtyAppService specialtyAppService,
            ICommonTraitAppService commonTraitAppService, IDegreeRepository degreeRepository,
            IOptions<BaseFilterOrderConfig> baseFilterConfig)
        {
            _catalogAppService = catalogAppService;
            _specialtyAppService = specialtyAppService;
            _commonTraitAppService = commonTraitAppService;
            _degreeRepository = degreeRepository;
            _baseFilterConfig = baseFilterConfig;
        }

        // Compatibility with old urls
        [AllowAnonymous]
        [HttpGet("/api/catalog-filters/{lang}/university")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public Task<IActionResult> GetCatalogByLangAndEntityTypeCompat(string lang)
        {
            return GetCatalogByLangAndEntityType(lang);
        }

        // Compatibility with old urls
        [AllowAnonymous]
        [HttpGet("/api/catalog-filters/{lang}/university/search")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public Task<IActionResult> SearchInCatalogCompat(string lang, [FromBody] CatalogSearchDto catalogSearchDto)
        {
            return SearchInCatalog(lang, catalogSearchDto);
        }

        [AllowAnonymous]
        [HttpGet("/api/catalog/university/filters/{lang}")]
        public async Task<IActionResult> GetCatalogByLangAndEntityType(string lang)
        {
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

            var universityDtos = catalogPage.Item2.Select(university => new CatalogUniversityDto
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

        private string FormatPriceValue(int price, string lang)
        {
            return lang == "ru"
                ? $"До {price} евро"
                : lang == "fr"
                    ? $"Jusqu'à {price} euros"
                    : lang == "esp"
                        ? $"Hasta {price} euros"
                        : $"Up to {price} euros";
        }

        private string FormatPrice(int group, string lang)
        {
            return FormatPriceValue(CostGroup.GetCostGroup(group).to, lang);
        }
    }
}