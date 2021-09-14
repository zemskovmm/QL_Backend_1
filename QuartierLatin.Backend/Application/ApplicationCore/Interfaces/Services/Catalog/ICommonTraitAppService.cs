using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.Catalog
{
    public interface ICommonTraitAppService
    {
        Task<List<CommonTrait>> GetTraitOfTypesByTypeIdAsync(int typeId);
        Task<int> CreateCommonTraitAsync(int typeId, Dictionary<string, string> names, int order, int? iconBlobId, int? parentId);
        Task<CommonTrait> GetTraitByIdAsync(int id);
        Task UpdateCommonTraitAsync(int id, Dictionary<string, string> names, int commonTraitTypeId, int? iconBlobId, int order, int? parentId);
        Task<List<CommonTrait>> GetTraitOfTypesByTypeIdAndUniversityIdAsync(int typeId, int universityId);
        Task<Dictionary<int, Dictionary<CommonTraitType, List<CommonTrait>>>> GetTraitsForEntityIds(
            EntityType entityType,
            List<int> ids);
        Task<List<CommonTrait>> GetTraitOfTypesByTypeIdAndSchoolIdAsync(int traitTypeId, int schoolId);
        Task<List<CommonTrait>> GetTraitOfTypesByTypeIdAndCourseIdAsync(int traitTypeId, int courseId);
        Task<List<CommonTrait>> GetTraitOfTypesByIdentifierAsync(string traitIdentifier);
        Task<List<CommonTrait>> GetTraitOfTypesByTypeIdAndHousingIdAsync(int traitTypeId, int housingId);
    }
}
