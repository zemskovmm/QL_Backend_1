using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto.CommonTraitDto;
using QuartierLatin.Backend.Dto.TraitTypeDto;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Serialization;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.Catalog;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using RemoteUi;

namespace QuartierLatin.Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/api/admin")]
    public class AdminTraitController : Controller
    {
        private readonly ICommonTraitAppService _commonTraitAppService;
        private readonly ICommonTraitTypeAppService _commonTraitTypeAppService;
        private readonly JObject _definition;
        private readonly JObject _traitTypeDefinition;

        public AdminTraitController(ICommonTraitAppService commonTraitAppService,
            ICommonTraitTypeAppService commonTraitTypeAppService)
        {
            var noFields = Array.Empty<IExtraRemoteUiField>();
            
            _commonTraitAppService = commonTraitAppService;
            _commonTraitTypeAppService = commonTraitTypeAppService;
            
            _definition = new RemoteUiBuilder(typeof(CommonTraitDtoRemoteUI), noFields, null, new CamelCaseNamingStrategy())
                .Register(typeof(CommonTraitDtoRemoteUI), noFields)
                .Register(typeof(Dictionary<string, string>), noFields)
                .Build(null);

           _traitTypeDefinition =
                new RemoteUiBuilder(typeof(TraitTypeDto), noFields, null, new CamelCaseNamingStrategy())
				
				
                    .Register(typeof(TraitTypeDto), noFields)
                    .Register(typeof(EntityTypeDto), noFields)
                    .Register(typeof(Dictionary<string, string>), noFields)
                    .Build(null);
					
					
			
        }

        [HttpGet("trait-type/definition")]
        public async Task<IActionResult> GetTraitTypeDefinition() => Ok(_traitTypeDefinition);
        
        [HttpGet("trait/definition")]
        public async Task<IActionResult> GetDefinition() => Ok(_definition);

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
            var traitTypeEntityTypes = await _commonTraitTypeAppService.GetEntityTypesTraitTypeByIdAsync(id);
			var entityTypeDtoItems=traitTypeEntityTypes.Select(entityTypeTraitType =>  new EntityTypeDto
            {
						
				EntityTypeName = entityTypeTraitType.EntityType.ToString(),
                EntityTypeId = (int)entityTypeTraitType.EntityType	
				
				
				
			}).ToList();	
            var response = new TraitTypeDto
            {
                Identifier = traitType.Identifier,
                Names = traitType.Names,
				EntityTypes = entityTypeDtoItems
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

        [HttpGet("entity-traits-university/{universityId}/by-type/{commonTraitTypeId}")]
        public async Task<IActionResult> GetEntityTraitByTypeToUniversity(int universityId, int commonTraitTypeId)
        {
            var response = await _commonTraitTypeAppService.GetEntityTraitToUniversityIdByCommonTraitTypeIdListAsync(universityId, commonTraitTypeId);
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

        [HttpGet("entity-traits-school/{schoolId}/by-type/{commonTraitTypeId}")]
        public async Task<IActionResult> GetEntityTraitByTypeToSchool(int schoolId, int commonTraitTypeId)
        {
            var response = await _commonTraitTypeAppService.GetEntityTraitToSchoolIdByCommonTraitTypeIdListAsync(schoolId, commonTraitTypeId);
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

        [HttpGet("entity-traits-course/{courseId}/by-type/{commonTraitTypeId}")]
        public async Task<IActionResult> GetEntityTraitByTypeToCourse(int courseId, int commonTraitTypeId)
        {
            var response = await _commonTraitTypeAppService.GetEntityTraitToCourseIdByCommonTraitTypeIdListAsync(courseId, commonTraitTypeId);
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

        [HttpGet("entity-traits-page/{pageId}")]
        public async Task<IActionResult> GetEntityTraitToPage(int pageId)
        {
            var response = await _commonTraitTypeAppService.GetEntityTraitToPageIdListAsync(pageId);
            return Ok(response);
        }

        [HttpPost("entity-traits-page/{pageId}/{commonTraitId}")]
        public async Task<IActionResult> CreateEntityTraitToPage(int pageId, int commonTraitId)
        {
            await _commonTraitTypeAppService.CreateEntityTraitToPageAsync(pageId, commonTraitId);
            return Ok(new object());
        }

        [HttpDelete("entity-traits-page/{pageId}/{commonTraitId}")]
        public async Task<IActionResult> DeleteEntityTraitToPage(int pageId, int commonTraitId)
        {
            await _commonTraitTypeAppService.DeleteEntityTraitToPageAsync(pageId, commonTraitId);
            return Ok(new object());
        }

        [HttpGet("traits/by-type/{traitIdentifier}")]
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

        [HttpGet("entity-traits-housing/{housingId}")]
        public async Task<IActionResult> GetEntityTraitToHousing(int housingId)
        {
            var response = await _commonTraitTypeAppService.GetEntityTraitToHousingIdListAsync(housingId);
            return Ok(response);
        }

        [HttpPost("entity-traits-housing/{housingId}/{commonTraitId}")]
        public async Task<IActionResult> CreateEntityTraitToHousing(int housingId, int commonTraitId)
        {
            await _commonTraitTypeAppService.CreateEntityTraitToHousingAsync(housingId, commonTraitId);
            return Ok(new object());
        }

        [HttpDelete("entity-traits-housing/{housingId}/{commonTraitId}")]
        public async Task<IActionResult> DeleteEntityTraitToHousing(int housingId, int commonTraitId)
        {
            await _commonTraitTypeAppService.DeleteEntityTraitToHousingAsync(housingId, commonTraitId);
            return Ok(new object());
        }

        [HttpGet("entity-traits-housing-accommodation-type/{housingAccommodationTypeId}")]
        public async Task<IActionResult> GetEntityTraitToHousingAccommodationType(int housingAccommodationTypeId)
        {
            var response = await _commonTraitTypeAppService.GetEntityTraitToHousingAccommodationTypeIdListAsync(housingAccommodationTypeId);
            return Ok(response);
        }

        [HttpPost("entity-traits-housing-accommodation-type/{housingAccommodationTypeId}/{commonTraitId}")]
        public async Task<IActionResult> CreateEntityTraitToHousingAccommodationType(int housingAccommodationTypeId, int commonTraitId)
        {
            await _commonTraitTypeAppService.CreateEntityTraitToHousingAccommodationTypeAsync(housingAccommodationTypeId, commonTraitId);
            return Ok(new object());
        }

        [HttpDelete("entity-traits-housing-accommodation-type/{housingAccommodationTypeId}/{commonTraitId}")]
        public async Task<IActionResult> DeleteEntityTraitToHousingAccommodationType(int housingAccommodationTypeId, int commonTraitId)
        {
            await _commonTraitTypeAppService.DeleteEntityTraitToHousingAccommodationTypeAsync(housingAccommodationTypeId, commonTraitId);
            return Ok(new object());
        }
    }
}