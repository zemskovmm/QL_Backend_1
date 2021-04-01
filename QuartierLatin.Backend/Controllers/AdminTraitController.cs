using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.Interfaces.Catalog;
using QuartierLatin.Backend.Dto.CommonTraitDto;
using QuartierLatin.Backend.Dto.TraitTypeDto;
using QuartierLatin.Backend.Models.Enums;

namespace QuartierLatin.Backend.Controllers
{
    public class AdminTraitController : Controller
    {
        private readonly ICommonTraitAppService _commonTraitAppService;
        private readonly ICommonTraitTypeAppService _commonTraitTypeAppService;

        public AdminTraitController(ICommonTraitAppService commonTraitAppService,
            ICommonTraitTypeAppService commonTraitTypeAppService)
        {
            _commonTraitAppService = commonTraitAppService;
            _commonTraitTypeAppService = commonTraitTypeAppService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/api/admin/trait-types")]
        public async Task<IActionResult> GetTraitTypes()
        {
            var traitTypes = await _commonTraitTypeAppService.GetTraitTypesAsync();

            var response = traitTypes.Select(trait => new TraitTypeListDto
            {
                Id = trait.Id,
                Identifier = trait.Identifier,
                Names = JObject.Parse(trait.Names)
            });

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/api/admin/trait-types")]
        public async Task<IActionResult> CreateTraitTypes([FromBody] TraitTypeDto traitTypeDto)
        {
            var response =
                await _commonTraitTypeAppService.CreateTraitTypeAsync(traitTypeDto.Identifier, traitTypeDto.Names);

            return Ok(new {id = response});
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/api/admin/trait-types/{id}")]
        public async Task<IActionResult> GetTraitTypeById(int id)
        {
            var traitType = await _commonTraitTypeAppService.GetTraitTypeByIdAsync(id);

            var response = new TraitTypeDto
            {
                Identifier = traitType.Identifier,
                Names = JObject.Parse(traitType.Names)
            };

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("/api/admin/trait-types/{id}")]
        public async Task<IActionResult> UpdateTraitTypeById([FromBody] TraitTypeDto traitTypeDto, int id)
        {
            await _commonTraitTypeAppService.UpdateTraitTypeByIdAsync(id, traitTypeDto.Identifier, traitTypeDto.Names);

            return Ok(new object());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/api/admin/entity-trait-types/{entityType}")]
        public async Task<IActionResult> GetTraitTypeForEntityByEntityType(EntityType entityType)
        {
            var traitTypeForEntitiesByEntityType =
                await _commonTraitTypeAppService.GetTraitTypeForEntitiesByEntityTypeAsync(entityType);
            return Ok(traitTypeForEntitiesByEntityType);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/api/admin/entity-trait-types/{entityType}/{commonTraitId}")]
        public async Task<IActionResult> CreateTraitTypeForEntityByEntityType(EntityType entityType, int commonTraitId)
        {
            await _commonTraitTypeAppService.CreateTraitTypeForEntityByEntityTypeAsync(entityType, commonTraitId);
            return Ok(new object());
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("/api/admin/entity-trait-types/{entityType}/{commonTraitId}")]
        public async Task<IActionResult> DeleteTraitTypeForEntityByEntityType(EntityType entityType, int commonTraitId)
        {
            await _commonTraitTypeAppService.DeleteTraitTypeForEntityByEntityTypeAsync(entityType, commonTraitId);
            return Ok(new object());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/api/admin/traits/of-type/{typeId}")]
        public async Task<IActionResult> GetTraitOfTypeByTypeId(int typeId)
        {
            var traitList = await _commonTraitAppService.GetTraitOfTypesByTypeIdAsync(typeId);

            var response = traitList.Select(trait => new CommonTraitListDto
            {
                Id = trait.Id,
                CommonTraitTypeId = trait.CommonTraitTypeId,
                IconBlobId = trait.IconBlobId,
                Names = JObject.Parse(trait.Names),
                Order = trait.Order,
                ParentId = trait.ParentId
            });

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/api/admin/traits/of-type/{typeId}")]
        public async Task<IActionResult> CreateTraitOfTypeByTypeId([FromBody] CreateCommonTraitDto createCommonTraitDto,
            int typeId)
        {
            var response = await _commonTraitAppService.CreateCommonTraitAsync(typeId, createCommonTraitDto.Names,
                createCommonTraitDto.Order, createCommonTraitDto.IconBlobId, createCommonTraitDto.ParentId);
            return Ok(new {id = response});
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/api/admin/traits/{id}")]
        public async Task<IActionResult> GetTraitById(int id)
        {
            var trait = await _commonTraitAppService.GetTraitByIdAsync(id);

            var response = new CommonTraitDto
            {
                CommonTraitTypeId = trait.CommonTraitTypeId,
                IconBlobId = trait.IconBlobId,
                Names = JObject.Parse(trait.Names),
                Order = trait.Order,
                ParentId = trait.ParentId
            };

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("/api/admin/traits/{id}")]
        public async Task<IActionResult> UpdateTraitById([FromBody] CommonTraitDto commonTraitDto, int id)
        {
            await _commonTraitAppService.UpdateCommonTraitAsync(id, commonTraitDto.Names,
                commonTraitDto.CommonTraitTypeId,
                commonTraitDto.IconBlobId, commonTraitDto.Order, commonTraitDto.ParentId);
            return Ok(new object());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/api/admin/entity-traits-university/{universityId}")]
        public async Task<IActionResult> GetEntityTraitToUniversity(int universityId)
        {
            var response = await _commonTraitTypeAppService.GetEntityTraitToUniversityIdListAsync(universityId);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/api/admin/entity-traits-university/{universityId}/{commonTraitId}")]
        public async Task<IActionResult> CreateEntityTraitToUniversity(int universityId, int commonTraitId)
        {
            await _commonTraitTypeAppService.CreateEntityTraitToUniversityAsync(universityId, commonTraitId);
            return Ok(new object());
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("/api/admin/entity-traits-university/{universityId}/{commonTraitId}")]
        public async Task<IActionResult> DeleteEntityTraitToUniversity(int universityId, int commonTraitId)
        {
            await _commonTraitTypeAppService.DeleteEntityTraitToUniversityAsync(universityId, commonTraitId);
            return Ok(new object());
        }
    }
}