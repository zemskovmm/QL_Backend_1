using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto.CommonTraitDto;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.Catalog;

namespace QuartierLatin.Backend.Controllers
{
    [AllowAnonymous]
    [Route("/api/traits")]
    public class TraitController : Controller
    {
        private readonly ICommonTraitAppService _commonTraitAppService;

        public TraitController(ICommonTraitAppService commonTraitAppService)
        {
            _commonTraitAppService = commonTraitAppService;
        }

        [HttpGet("by-type/{traitIdentifier}")]
        public async Task<IActionResult> GetTraitOfTypeByTypeName(string traitIdentifier)
        {
            var traitList = await _commonTraitAppService.GetTraitOfTypesByIdentifierAsync(traitIdentifier);

            var response = traitList.Select(trait => new CommonTraitListDto
            {
                Id = trait.Id,
                CommonTraitTypeId = trait.CommonTraitTypeId,
                IconBlobId = trait.IconBlobId,
                Names = JObject.Parse(trait.NamesJson),
                Order = trait.Order,
                ParentId = trait.ParentId
            });

            return Ok(response);
        }
    }
}
