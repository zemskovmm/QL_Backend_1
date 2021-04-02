using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuartierLatin.Backend.Dto.CatalogDto;
using QuartierLatin.Backend.Dto.CatalogDto.CatalogSearchDto;
using QuartierLatin.Backend.Models.Enums;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.Interfaces.Catalog;

namespace QuartierLatin.Backend.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogAppService _catalogAppService;
        public CatalogController(ICatalogAppService catalogAppService)
        {
            _catalogAppService = catalogAppService;
        }

        [AllowAnonymous]
        [HttpGet("/api/catalog-filters/{lang}/{entityType}")]
        public async Task<IActionResult> GetCatalogByLangAndEntityType(string lang, EntityType entityType, [FromBody] CatalogFilterListDto catalogFilterListDto)
        {
            
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("/api/catalog-filters/{lang}/{entityType}/search")]
        public async Task<IActionResult> SearchInCatalog(string lang, EntityType entityType, [FromBody] CatalogSearchDto catalogSearchDto)
        {

            return Ok();
        }
    }
}
