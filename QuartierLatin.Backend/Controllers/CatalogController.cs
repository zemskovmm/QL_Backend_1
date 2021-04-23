using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Dto.CatalogDto;
using QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto;
using QuartierLatin.Backend.Models.Enums;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.Interfaces.Catalog;
using QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto.CatalogSearchResponseDto;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;

namespace QuartierLatin.Backend.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogAppService _catalogAppService;
        private readonly ICommonTraitAppService _commonTraitAppService;

        public CatalogController(ICatalogAppService catalogAppService, ICommonTraitAppService commonTraitAppService)
        {
            _catalogAppService = catalogAppService;
            _commonTraitAppService = commonTraitAppService;
        }

        [AllowAnonymous]
        [HttpGet("/api/catalog-filters/{lang}/{entityType}")]
        public async Task<IActionResult> GetCatalogByLangAndEntityType(string lang, EntityType entityType)
        {
            var commonTraits = await _catalogAppService.GetNamedCommonTraitsAndTraitTypeByEntityType(entityType);

            var priceLangVersion = new Dictionary<string, (string, string, string, string)>
            {
                {"en", ("Price", "Up to 10000 euros", "Up to 20000 euros", "Up to 30000 euros")},
                {"ru", ("Стоимость", "До 10000 евро", "До 20000 евро", "До 30000 евро")},
                {"fr", ("Le coût", "Jusqu'à 10000 euros", "Jusqu'à 20000 euros", "Jusqu'à 30000 euros")}, 
                {"esp", ("El costo", "Hasta 10000 euros", "Hasta 20000 euros", "Hasta 30000 euros")}
            };

            var filters = commonTraits.Select(trait => new CatalogFilterDto
            {
                Name = trait.Item1.Names[lang],
                Identifier = trait.Item1.Identifier,
                Options = trait.Item2.Select(commonTrait => new CatalogOptionsDto
                {
                    Name = commonTrait.Names[lang],
                    Id = commonTrait.Id
                }).ToList()
            }).ToList();

            filters.Add(new CatalogFilterDto
            {
                Identifier = "price",
                Name = priceLangVersion[lang].Item1,
                Options = new List<CatalogOptionsDto>
                {
                    new CatalogOptionsDto
                    {
                        Name = priceLangVersion[lang].Item2
                    },
                    new CatalogOptionsDto
                    {
                        Name =  priceLangVersion[lang].Item3
                    },
                    new CatalogOptionsDto
                    {
                        Name = priceLangVersion[lang].Item4
                    },
                }
            });

            var response = new CatalogFilterResponseDto
            {
                Filters = filters
            };

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("/api/catalog-filters/{lang}/{entityType}/search")]
        public async Task<IActionResult> SearchInCatalog(string lang, EntityType entityType, [FromBody] CatalogSearchDto catalogSearchDto)
        {
            const int pageSize = 10;
            var commonTraits =
                catalogSearchDto.Filters.ToDictionary(filter => new CommonTraitType {Identifier = filter.Identifier}, filter =>
                    filter.Values.Select(traits => new CommonTrait{Id = traits}).ToList());

            var catalogPage =
                await _catalogAppService.GetCatalogPageByFilter(lang, entityType, commonTraits,
                    catalogSearchDto.PageNumber, pageSize);

            var traitDic = await _commonTraitAppService.GetTraitsForEntityIds(EntityType.University,
                catalogPage.Item2.Select(x => x.Item1.Id).ToList());

            List<CommonTrait> GetTraits(string identifier, int id)
            {
                if (!traitDic.TryGetValue(id, out var traits))
                    return new List<CommonTrait>();
                return traits.FirstOrDefault(x => x.Key.Identifier == identifier).Value ?? new List<CommonTrait>();
            }

            string GetName(CommonTrait trait, string lang)
            {
                return trait.Names.GetValueOrDefault(lang) ??
                    trait.Names.GetValueOrDefault("en") ?? trait.Names.FirstOrDefault().Value;
            }

            var universityDtos = catalogPage.Item2.Select(university => new CatalogUniversityDto()
            {
                Url = $"/{lang}/university/{university.Item2.Url}",
                Name = university.Item2.Name,
                PriceFrom = university.cost,
                PriceTo = university.cost,
                Degrees = GetTraits("degree", university.Item1.Id).Select(x => GetName(x, lang)).ToList(),
                InstructionLanguages = GetTraits("instruction-language", university.Item1.Id).Select(x => x.Identifier)
                    .ToList()
            }).ToList();

            var response = new CatalogSearchResponseDtoList<CatalogUniversityDto>
            {
                Items = universityDtos,
                TotalPages = catalogPage.totalPages
            };

            return Ok(response);
        }
    }
}
