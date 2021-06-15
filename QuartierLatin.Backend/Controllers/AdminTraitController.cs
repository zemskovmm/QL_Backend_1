using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.Interfaces.Catalog;
using QuartierLatin.Backend.Dto.CommonTraitDto;
using QuartierLatin.Backend.Dto.TraitTypeDto;
using QuartierLatin.Backend.Models.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/api/admin")]
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

        [HttpGet("trait-types")]
        public async Task<IActionResult> GetTraitTypes()
        {
            var traitTypes = await _commonTraitTypeAppService.GetTraitTypesAsync();

            var response = traitTypes.Select(trait => new TraitTypeListDto
            {
                Id = trait.Id,
                Identifier = trait.Identifier,
                Names = JObject.Parse(trait.NamesJson)
            });

            return Ok(response);
        }

        
        [HttpPost("trait-types")]
        public async Task<IActionResult> CreateTraitTypes([FromBody] TraitTypeDto traitTypeDto)
        {
            var response =
                await _commonTraitTypeAppService.CreateTraitTypeAsync(traitTypeDto.Identifier, traitTypeDto.Names, traitTypeDto.Order);

            return Ok(new {id = response});
        }

        
        [HttpGet("trait-types/{id}")]
        public async Task<IActionResult> GetTraitTypeById(int id)
        {
            var traitType = await _commonTraitTypeAppService.GetTraitTypeByIdAsync(id);

            var response = new TraitTypeDto
            {
                Identifier = traitType.Identifier,
                Names = traitType.Names
            };

            return Ok(response);
        }

        
        [HttpPut("trait-types/{id}")]
        public async Task<IActionResult> UpdateTraitTypeById([FromBody] TraitTypeDto traitTypeDto, int id)
        {
            await _commonTraitTypeAppService.UpdateTraitTypeByIdAsync(id, traitTypeDto.Identifier, traitTypeDto.Names, traitTypeDto.Order);

            return Ok(new object());
        }

        
        [HttpGet("entity-trait-types/{entityType}")]
        public async Task<IActionResult> GetTraitTypeForEntityByEntityType(EntityType entityType)
        {
            var traitTypeForEntitiesByEntityType =
                await _commonTraitTypeAppService.GetTraitTypeForEntitiesByEntityTypeAsync(entityType);
            return Ok(traitTypeForEntitiesByEntityType);
        }

        
        [HttpPost("entity-trait-types/{entityType}/{commonTraitId}")]
        public async Task<IActionResult> CreateTraitTypeForEntityByEntityType(EntityType entityType, int commonTraitId)
        {
            await _commonTraitTypeAppService.CreateTraitTypeForEntityByEntityTypeAsync(entityType, commonTraitId);
            return Ok(new object());
        }

        
        [HttpDelete("entity-trait-types/{entityType}/{commonTraitId}")]
        public async Task<IActionResult> DeleteTraitTypeForEntityByEntityType(EntityType entityType, int commonTraitId)
        {
            await _commonTraitTypeAppService.DeleteTraitTypeForEntityByEntityTypeAsync(entityType, commonTraitId);
            return Ok(new object());
        }

        
        [HttpGet("traits/of-type/{typeId}")]
        public async Task<IActionResult> GetTraitOfTypeByTypeId(int typeId)
        {
            var traitList = await _commonTraitAppService.GetTraitOfTypesByTypeIdAsync(typeId);

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

        
        [HttpPost("traits/of-type/{typeId}")]
        public async Task<IActionResult> CreateTraitOfTypeByTypeId([FromBody] CreateCommonTraitDto createCommonTraitDto,
            int typeId)
        {
            var response = await _commonTraitAppService.CreateCommonTraitAsync(typeId, createCommonTraitDto.Names,
                createCommonTraitDto.Order, createCommonTraitDto.IconBlobId, createCommonTraitDto.ParentId);
            return Ok(new {id = response});
        }

        
        [HttpGet("traits/{id}")]
        public async Task<IActionResult> GetTraitById(int id)
        {
            var trait = await _commonTraitAppService.GetTraitByIdAsync(id);

            var response = new CommonTraitDto
            {
                CommonTraitTypeId = trait.CommonTraitTypeId,
                IconBlobId = trait.IconBlobId,
                Names = trait.Names,
                Order = trait.Order,
                ParentId = trait.ParentId
            };

            return Ok(response);
        }

        
        [HttpPut("traits/{id}")]
        public async Task<IActionResult> UpdateTraitById([FromBody] CommonTraitDto commonTraitDto, int id)
        {
            await _commonTraitAppService.UpdateCommonTraitAsync(id, commonTraitDto.Names,
                commonTraitDto.CommonTraitTypeId,
                commonTraitDto.IconBlobId, commonTraitDto.Order, commonTraitDto.ParentId);
            return Ok(new object());
        }

        
        [HttpGet("entity-traits-university/{universityId}")]
        public async Task<IActionResult> GetEntityTraitToUniversity(int universityId)
        {
            var response = await _commonTraitTypeAppService.GetEntityTraitToUniversityIdListAsync(universityId);
            return Ok(response);
        }

        
        [HttpPost("entity-traits-university/{universityId}/{commonTraitId}")]
        public async Task<IActionResult> CreateEntityTraitToUniversity(int universityId, int commonTraitId)
        {
            await _commonTraitTypeAppService.CreateEntityTraitToUniversityAsync(universityId, commonTraitId);
            return Ok(new object());
        }

        
        [HttpDelete("entity-traits-university/{universityId}/{commonTraitId}")]
        public async Task<IActionResult> DeleteEntityTraitToUniversity(int universityId, int commonTraitId)
        {
            await _commonTraitTypeAppService.DeleteEntityTraitToUniversityAsync(universityId, commonTraitId);
            return Ok(new object());
        }

        
        [HttpGet("entity-traits-school/{schoolId}")]
        public async Task<IActionResult> GetEntityTraitToSchool(int schoolId)
        {
            var response = await _commonTraitTypeAppService.GetEntityTraitToSchoolIdListAsync(schoolId);
            return Ok(response);
        }

        
        [HttpPost("entity-traits-school/{schoolId}/{commonTraitId}")]
        public async Task<IActionResult> CreateEntityTraitToSchool(int schoolId, int commonTraitId)
        {
            await _commonTraitTypeAppService.CreateEntityTraitToSchoolAsync(schoolId, commonTraitId);
            return Ok(new object());
        }

        
        [HttpDelete("entity-traits-school/{schoolId}/{commonTraitId}")]
        public async Task<IActionResult> DeleteEntityTraitToSchool(int schoolId, int commonTraitId)
        {
            await _commonTraitTypeAppService.DeleteEntityTraitToSchoolAsync(schoolId, commonTraitId);
            return Ok(new object());
        }

        
        [HttpGet("entity-traits-course/{courseId}")]
        public async Task<IActionResult> GetEntityTraitToCourse(int courseId)
        {
            var response = await _commonTraitTypeAppService.GetEntityTraitToCourseIdListAsync(courseId);
            return Ok(response);
        }

        
        [HttpPost("entity-traits-course/{courseId}/{commonTraitId}")]
        public async Task<IActionResult> CreateEntityTraitToCourse(int courseId, int commonTraitId)
        {
            await _commonTraitTypeAppService.CreateEntityTraitToCourseAsync(courseId, commonTraitId);
            return Ok(new object());
        }

        
        [HttpDelete("entity-traits-course/{courseId}/{commonTraitId}")]
        public async Task<IActionResult> DeleteEntityTraitToCourse(int courseId, int commonTraitId)
        {
            await _commonTraitTypeAppService.DeleteEntityTraitToCourseAsync(courseId, commonTraitId);
            return Ok(new object());
        }
    }
}