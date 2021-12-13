using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.Catalog;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;

namespace QuartierLatin.Backend.Services.Catalog
{
    public class CommonTraitTypeAppService : ICommonTraitTypeAppService
    {
        private readonly ICommonTraitTypeRepository _commonTraitTypeRepository;

        public CommonTraitTypeAppService(ICommonTraitTypeRepository commonTraitTypeRepository)
        {
            _commonTraitTypeRepository = commonTraitTypeRepository;
        }

        public async Task<List<CommonTraitType>> GetTraitTypesAsync()
        {
            return await _commonTraitTypeRepository.GetCommonTraitTypeListAsync();
        }
		
		
		public async Task<List<CommonTraitTypesForEntity>> GetEntityTypesTraitTypeByIdAsync(int id)
		{
			
            return await _commonTraitTypeRepository.GetEntityTypesTraitTypeByIdAsync(id);			
		}

        public async Task<int> CreateTraitTypeAsync(string? identifier, Dictionary<string, string> names, int order)
        {
            return await _commonTraitTypeRepository.CreateCommonTraitTypeAsync(names, identifier, order);
        }

        public async Task<CommonTraitType> GetTraitTypeByIdAsync(int id)
        {
            return await _commonTraitTypeRepository.GetCommonTraitTypeAsync(id);
        }

        public async Task UpdateTraitTypeByIdAsync(int id, string? identifier, Dictionary<string, string> names, int order)
        {
            await _commonTraitTypeRepository.UpdateCommonTraitTypeAsync(id, names, identifier, order);
        }

        public async Task<IEnumerable<int>> GetTraitTypeForEntitiesByEntityTypeAsync(EntityType entityType)
        {
            return await _commonTraitTypeRepository.GetTraitTypeForEntitiesByEntityTypeIdListAsync(entityType);
        }

        public async Task CreateTraitTypeForEntityByEntityTypeAsync(EntityType entityType, int traitTypeId)
        {
            await _commonTraitTypeRepository.CreateOrUpdateCommonTraitTypesForEntityAsync(traitTypeId, entityType);
        }

        public async Task DeleteTraitTypeForEntityByEntityTypeAsync(EntityType entityType, int traitTypeId)
        {
            await _commonTraitTypeRepository.DeleteCommonTraitTypesForEntityAsync(traitTypeId, entityType);
        }

        public async Task<List<int>> GetEntityTraitToUniversityIdListAsync(int universityId)
        {
            return await _commonTraitTypeRepository.GetEntityTraitToUniversityIdListAsync(universityId);
        }

        public async Task CreateEntityTraitToUniversityAsync(int universityId, int commonTraitId)
        {
            await _commonTraitTypeRepository.CreateEntityTraitToUniversityAsync(universityId, commonTraitId);
        }

        public async Task DeleteEntityTraitToUniversityAsync(int universityId, int commonTraitId)
        {
            await _commonTraitTypeRepository.DeleteEntityTraitToUniversityAsync(universityId, commonTraitId);
        }

        public async Task<List<CommonTraitType>> GetTraitTypesWithIndetifierAsync()
        {
            return await _commonTraitTypeRepository.GetTraitTypesWithIndetifierAsync();
        }

        public async Task<List<int>> GetEntityTraitToSchoolIdListAsync(int schoolId)
        {
            return await _commonTraitTypeRepository.GetEntityTraitToSchoolIdListAsync(schoolId);
        }

        public async Task CreateEntityTraitToSchoolAsync(int schoolId, int commonTraitId)
        {
            await _commonTraitTypeRepository.CreateEntityTraitToSchoolAsync(schoolId, commonTraitId);
        }

        public async Task DeleteEntityTraitToSchoolAsync(int schoolId, int commonTraitId)
        {
            await _commonTraitTypeRepository.DeleteEntityTraitToSchoolAsync(schoolId, commonTraitId);
        }

        public async Task<List<int>> GetEntityTraitToCourseIdListAsync(int courseId)
        {
            return await _commonTraitTypeRepository.GetEntityTraitToCourseIdListAsync(courseId);
        }

        public async Task CreateEntityTraitToCourseAsync(int courseId, int commonTraitId)
        {
            await _commonTraitTypeRepository.CreateEntityTraitToCourseAsync(courseId, commonTraitId);
        }

        public async Task DeleteEntityTraitToCourseAsync(int courseId, int commonTraitId)
        {
            await _commonTraitTypeRepository.DeleteEntityTraitToCourseAsync(courseId, commonTraitId);
        }

        public async Task<List<int>> GetEntityTraitToUniversityIdByCommonTraitTypeIdListAsync(int universityId, int commonTraitTypeId)
        {
            return await _commonTraitTypeRepository.GetEntityTraitToUniversityIdByCommonTraitTypeIdListAsync(universityId,
                commonTraitTypeId);
        }

        public async Task<List<int>> GetEntityTraitToSchoolIdByCommonTraitTypeIdListAsync(int schoolId, int commonTraitTypeId)
        {
            return await _commonTraitTypeRepository.GetEntityTraitToSchoolIdByCommonTraitTypeIdListAsync(schoolId,
                commonTraitTypeId);
        }

        public async Task<List<int>> GetEntityTraitToCourseIdByCommonTraitTypeIdListAsync(int courseId,
            int commonTraitTypeId)
        {
            return await _commonTraitTypeRepository.GetEntityTraitToCourseIdByCommonTraitTypeIdListAsync(courseId,
                commonTraitTypeId);
        }

        public async Task<List<int>> GetEntityTraitToPageIdListAsync(int pageId)
        {
            return await _commonTraitTypeRepository.GetEntityTraitToPageIdListAsync(pageId);
        }

        public async Task CreateEntityTraitToPageAsync(int pageId, int commonTraitId)
        {
            await _commonTraitTypeRepository.CreateEntityTraitToPageAsync(pageId, commonTraitId);
        }

        public async Task DeleteEntityTraitToPageAsync(int pageId, int commonTraitId)
        {
            await _commonTraitTypeRepository.DeleteEntityTraitToPageAsync(pageId, commonTraitId);
        }

        public async Task<List<int>> GetEntityTraitToHousingIdListAsync(int housingId)
        {
            return await _commonTraitTypeRepository.GetEntityTraitToHousingIdListAsync(housingId);
        }

        public async Task CreateEntityTraitToHousingAsync(int housingId, int commonTraitId)
        {
            await _commonTraitTypeRepository.CreateEntityTraitToHousingAsync(housingId, commonTraitId);
        }

        public async Task DeleteEntityTraitToHousingAsync(int housingId, int commonTraitId)
        {
            await _commonTraitTypeRepository.DeleteEntityTraitToHousingAsync(housingId, commonTraitId);
        }

        public async Task<List<int>> GetEntityTraitToHousingAccommodationTypeIdListAsync(int housingAccommodationTypeId)
        {
            return await _commonTraitTypeRepository.GetEntityTraitToHousingAccommodationTypeIdListAsync(housingAccommodationTypeId);
        }

        public async Task CreateEntityTraitToHousingAccommodationTypeAsync(int housingAccommodationTypeId, int commonTraitId)
        {
            await _commonTraitTypeRepository.CreateEntityTraitToHousingAccommodationTypeAsync(housingAccommodationTypeId, commonTraitId);
        }

        public async Task DeleteEntityTraitToHousingAccommodationTypeAsync(int housingAccommodationTypeId, int commonTraitId)
        {
            await _commonTraitTypeRepository.DeleteEntityTraitToHousingAccommodationTypeAsync(housingAccommodationTypeId, commonTraitId);
        }
    }
}