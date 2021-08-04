using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Application.Interfaces.Catalog;
using QuartierLatin.Backend.Dto.CatalogDto;
using QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto.CatalogSearchResponseDto;
using QuartierLatin.Backend.Dto.PageModuleDto;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Backend.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PageDto = QuartierLatin.Backend.Dto.PageModuleDto.PageDto;

namespace QuartierLatin.Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/api/pages")]
    public class PageController : Controller
    {
        private readonly IPageAppService _pageAppService;
        private readonly ICatalogAppService _catalogAppService;

        public PageController(IPageAppService pageAppService, ICatalogAppService catalogAppService)
        {
            _pageAppService = pageAppService;
            _catalogAppService = catalogAppService;
        }

        [AllowAnonymous]
        [HttpPost("search/{lang}")]
        public async Task<IActionResult> SearchInPages(string lang, [FromBody] PageSearchDto pageSearchDto)
        {
            var entityType = pageSearchDto.PageType;//(PageType)Enum.Parse(typeof(PageType), pageSearchDto.PageType);
            var pageSize = pageSearchDto.PageSize ?? 1000;
            var commonTraits =
                pageSearchDto.Filters.ToDictionary(filter => filter.Identifier, filter =>
                    filter.Values);

            var catalogPage =
                await _pageAppService.GetPagesByFilter(lang, entityType, commonTraits,
                    pageSearchDto.PageNumber, pageSize);

            var pageIds = catalogPage.Item2.Select(x => x.Item1.Id).ToList();

            var pageDtos = new List<PageDto>();

            foreach (var page in catalogPage.Item2)
            {
                pageDtos.Add(new PageDto(page.page.Title, JObject.Parse(page.page.PageData), page.page.Date,
                    page.pageRoot.PageType, page.page.PreviewImageId));
            };

            var response = new CatalogSearchResponseDtoList<PageDto>
            {
                Items = pageDtos,
                TotalItems = catalogPage.totalItems,
                TotalPages = FilterHelper.PageCount(catalogPage.totalItems, pageSize)
            };

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("filters/{lang}")]
        public async Task<IActionResult> GetPageFiltersByLang(string lang)
        {
            var entityType = EntityType.Page;

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
    }
}
