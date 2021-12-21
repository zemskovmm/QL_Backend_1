using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;
using QuartierLatin.Backend.Dto.CatalogDto;
using QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto.CatalogSearchResponseDto;
using QuartierLatin.Backend.Dto.PageModuleDto;
using QuartierLatin.Backend.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.Catalog;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Dto.CommonTraitDto;
using PageDto = QuartierLatin.Backend.Dto.PageModuleDto.PageDto;

namespace QuartierLatin.Backend.Controllers
{
    [AllowAnonymous]
    [Route("/api/pages")]
    public class PageController : Controller
    {
        private readonly IPageAppService _pageAppService;
        private readonly ICatalogAppService _catalogAppService;
        private readonly ICommonTraitTypeAppService _commonTraitTypeAppService;

        public PageController(IPageAppService pageAppService, ICatalogAppService catalogAppService,
            ICommonTraitTypeAppService commonTraitTypeAppService)
        {
            _pageAppService = pageAppService;
            _catalogAppService = catalogAppService;
            _commonTraitTypeAppService = commonTraitTypeAppService;
        }

        [HttpPost("search/{lang}")]
        public async Task<IActionResult> SearchInPages(string lang, [FromBody] PageSearchDto pageSearchDto, string date = null)
        {
            lang = lang.ToLower();

            var entityType = pageSearchDto.PageType;
            var pageSize = pageSearchDto.PageSize ?? 1000;
            var commonTraits =
                pageSearchDto.Filters.ToDictionary(filter => filter.Identifier, filter =>
                    filter.Values);

            var catalogPage =
                await _pageAppService.GetPagesByFilter(lang, entityType, commonTraits,
                    pageSearchDto.PageNumber, pageSize);

            var pageIds = catalogPage.Item2.Select(x => x.Item1.Id).ToList();

            var commonTraitsPages = await _pageAppService.GetCommonTraitListByPageIds(pageIds);

            var traitsType = await _commonTraitTypeAppService.GetTraitTypesWithIndetifierAsync();

            var pageDtos = new List<PageDto>();

            foreach (var page in catalogPage.Item2.OrderByDescending(x => x.page.Date))
            {
                var traits = new Dictionary<string, List<CommonTraitLanguageDto>>();

                foreach (var traitType in traitsType)
                {
                    var pageTraitTypedList = commonTraitsPages.GetValueOrDefault(page.pageRoot.Id)
                        ?.Where(type => type.CommonTraitTypeId == traitType.Id);

                    if (pageTraitTypedList is null) continue;

                    var pageTraitList = pageTraitTypedList.Select(page => new CommonTraitLanguageDto
                    {
                        Id = page.Id,
                        IconBlobId = page.IconBlobId,
                        Identifier = page.Identifier,
                        Name = page.Names.GetSuitableName(lang)
                    }).ToList();

                    traits.Add(traitType.Identifier, pageTraitList);
                }
                var block_items=JObject.Parse(page.page.PageData).SelectToken("rows[0].blocks[0].data.date");
				
				var rows_items=JObject.Parse(page.page.PageData).SelectToken("rows");
				DateTime? blockDate=null;
				foreach(var cur_item in rows_items){
					var blocks_array=cur_item.SelectToken("blocks");
					foreach(var cur_block in blocks_array){
						  if(cur_block.SelectToken("type").ToString()=="firstArticleBlock"){
								   blockDate=DateTime.ParseExact(cur_block.SelectToken("data.date").ToString(), "dd.MM.yyyy", null);
                                   break;								   
						  }							  
						  
					}
				}

                pageDtos.Add(new PageDto(page.page.Title, JObject.Parse(page.page.PageData), page.page.Date,
                    page.pageRoot.PageType, page.page.PreviewImageId, page.page.SmallPreviewImageId, page.page.WidePreviewImageId, traits, page.page.Url,
                    page.page.Metadata is null ? null : JObject.Parse(page.page.Metadata),blockDate));
            };
           
		    
            
            var response = new CatalogSearchResponseDtoList<PageDto>
            {
                Items = pageDtos.OrderByDescending(x => x.BlockDate).ToList<PageDto>(),
                TotalItems = catalogPage.totalItems,
                TotalPages = FilterHelper.PageCount(catalogPage.totalItems, pageSize)
            };

            return Ok(response);
        }

        [HttpGet("filters/{lang}")]
        public async Task<IActionResult> GetPageFiltersByLang(string lang)
        {
            lang = lang.ToLower();

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
