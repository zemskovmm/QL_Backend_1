using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CatalogRepositoies
{
    public interface ICommonTraitRepository
    {
        Task<int> CreateCommonTraitAsync(int commonTraitTypeId, Dictionary<string, string> names, int? iconBlobId, int order, int? parentId);
        Task CreateOrUpdateCommonTraitToUniversityAsync(int universityId, int commonTraitId);
        Task UpdateCommonTraitAsync(int id, int commonTraitTypeId, Dictionary<string, string> names, int? iconBlobId, int order, int? parentId);
        Task DeleteCommonTraitAsync(int id);
        Task DeleteCommonTraitToUniversityAsync(int universityId, int commonTraitId);
        Task<CommonTrait> GetCommonTraitAsync(int id);
        Task<List<CommonTrait>> GetCommonTraitListByTypeId(int typeId);
        Task<List<CommonTrait>> GetCommonTraitListByTypeIds(int[] typeId);
        Task<List<CommonTrait>> GetCommonTraitListByTypeIdAndUniversityId(int typeId, int universityId);
        Task<Dictionary<int, List<CommonTrait>>> GetCommonTraitListByUniversityIds(IEnumerable<int> ids);
        Task<Dictionary<int, List<CommonTrait>>> GetCommonTraitListByPageIds(IEnumerable<int> ids);
        Task<List<CommonTrait>> GetCommonTraitListByTypeIdAndSchoolIdAsync(int traitTypeId, int schoolId);
        Task<List<CommonTrait>> GetTraitOfTypesByTypeIdAndCourseIdAsync(int traitTypeId, int courseId);
        Task<List<CommonTrait>> GetTraitOfTypesByIdentifierAsync(string traitIdentifier);
        Task<Dictionary<int, List<CommonTrait>>> GetCommonTraitListByCourseIdsAsync(IEnumerable<int> courseIds);
        Task<List<CommonTrait>> GetTraitOfTypesByTypeIdAndHousingIdAsync(int traitTypeId, int housingId);
        Task<Dictionary<int, List<CommonTrait>>> GetCommonTraitListByHousingIdsAsync(IEnumerable<int> housingIds);
        Task<Dictionary<int, List<CommonTrait>>> GetCommonTraitListByHousingAccommodationTypeIdsAsync(IEnumerable<int> housingAccommodationIds);
    }
}
