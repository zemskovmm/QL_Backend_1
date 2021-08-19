using QuartierLatin.Backend.Application.Interfaces.Catalog;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Application.Catalog
{
    public class CommonTraitAppService : ICommonTraitAppService
    {
        private readonly ICommonTraitRepository _commonTraitRepository;
        private readonly ICommonTraitTypeRepository _commonTraitTypeRepository;

        public CommonTraitAppService(ICommonTraitRepository commonTraitRepository, 
            ICommonTraitTypeRepository commonTraitTypeRepository)
        {
            _commonTraitRepository = commonTraitRepository;
            _commonTraitTypeRepository = commonTraitTypeRepository;
        }

        public async Task<List<CommonTrait>> GetTraitOfTypesByTypeIdAsync(int typeId)
        {
            return await _commonTraitRepository.GetCommonTraitListByTypeId(typeId);
        }

        public async Task<int> CreateCommonTraitAsync(int typeId, Dictionary<string, string> names, int order, int? iconBlobId, int? parentId)
        {
            return await _commonTraitRepository.CreateCommonTraitAsync(typeId, names, iconBlobId, order, parentId);
        }

        public async Task<CommonTrait> GetTraitByIdAsync(int id)
        {
            return await _commonTraitRepository.GetCommonTraitAsync(id);
        }

        public async Task UpdateCommonTraitAsync(int id, Dictionary<string, string> names, int commonTraitTypeId, int? iconBlobId,
            int order, int? parentId)
        {
            await _commonTraitRepository.UpdateCommonTraitAsync(id, commonTraitTypeId, names, iconBlobId, order, parentId);
        }

        public async Task<List<CommonTrait>> GetTraitOfTypesByTypeIdAndUniversityIdAsync(int typeId, int universityId)
        {
            return await _commonTraitRepository.GetCommonTraitListByTypeIdAndUniversityId(typeId, universityId);
        }

        public async Task<Dictionary<int, Dictionary<CommonTraitType, List<CommonTrait>>>> GetTraitsForEntityIds(
            EntityType entityType, List<int> ids)
        {
            ids = ids.Distinct().ToList();
            var allTypes = await _commonTraitTypeRepository.GetCommonTraitTypeListAsync();
            var allTraits = await _commonTraitRepository.GetCommonTraitListByUniversityIds(ids);
            return ids.ToDictionary(x => x,
                entityId => allTypes.ToDictionary(traitType => traitType,
                    traitType =>
                        allTraits.GetValueOrDefault(entityId)?.Where(trait => trait.CommonTraitTypeId == traitType.Id)
                            .ToList() ??
                        new List<CommonTrait>()));
        }

        public async Task<List<CommonTrait>> GetTraitOfTypesByTypeIdAndSchoolIdAsync(int traitTypeId, int schoolId)
        {
            return await _commonTraitRepository.GetCommonTraitListByTypeIdAndSchoolIdAsync(traitTypeId, schoolId);
        }

        public async Task<List<CommonTrait>> GetTraitOfTypesByTypeIdAndCourseIdAsync(int traitTypeId, int courseId)
        {
            return await _commonTraitRepository.GetTraitOfTypesByTypeIdAndCourseIdAsync(traitTypeId, courseId);
        }

        public async Task<List<CommonTrait>> GetTraitOfTypesByIdentifierAsync(string traitIdentifier)
        {
            return await _commonTraitRepository.GetTraitOfTypesByIdentifierAsync(traitIdentifier);
        }
    }
}