using QuartierLatin.Backend.Application.Interfaces.Catalog;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.Catalog
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

        public async Task<List<int>> GetEntityTraitToCurseIdListAsync(int curseId)
        {
            return await _commonTraitTypeRepository.GetEntityTraitToCurseIdListAsync(curseId);
        }

        public async Task CreateEntityTraitToCurseAsync(int curseId, int commonTraitId)
        {
            await _commonTraitTypeRepository.CreateEntityTraitToCurseAsync(curseId, commonTraitId);
        }

        public async Task DeleteEntityTraitToCurseAsync(int curseId, int commonTraitId)
        {
            await _commonTraitTypeRepository.DeleteEntityTraitToCurseAsync(curseId, commonTraitId);
        }
    }
}