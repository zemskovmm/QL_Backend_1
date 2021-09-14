using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;

namespace QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services.Catalog
{
    public interface ICommonTraitTypeAppService
    {
        Task<List<CommonTraitType>> GetTraitTypesAsync();
        Task<int> CreateTraitTypeAsync(string? identifier, Dictionary<string, string> names, int order);
        Task<CommonTraitType> GetTraitTypeByIdAsync(int id);
        Task UpdateTraitTypeByIdAsync(int id, string? identifier, Dictionary<string, string> names, int order);
        Task<IEnumerable<int>> GetTraitTypeForEntitiesByEntityTypeAsync(EntityType entityType);
        Task CreateTraitTypeForEntityByEntityTypeAsync(EntityType entityType, int traitTypeId);
        Task DeleteTraitTypeForEntityByEntityTypeAsync(EntityType entityType, int traitTypeId);
        Task<List<int>> GetEntityTraitToUniversityIdListAsync(int universityId);
        Task CreateEntityTraitToUniversityAsync(int universityId, int commonTraitId);
        Task DeleteEntityTraitToUniversityAsync(int universityId, int commonTraitId);
        Task<List<CommonTraitType>> GetTraitTypesWithIndetifierAsync();
        Task<List<int>> GetEntityTraitToSchoolIdListAsync(int schoolId);
        Task CreateEntityTraitToSchoolAsync(int schoolId, int commonTraitId);
        Task DeleteEntityTraitToSchoolAsync(int schoolId, int commonTraitId);
        Task<List<int>> GetEntityTraitToCourseIdListAsync(int courseId);
        Task CreateEntityTraitToCourseAsync(int courseId, int commonTraitId);
        Task DeleteEntityTraitToCourseAsync(int courseId, int commonTraitId);
        Task<List<int>> GetEntityTraitToUniversityIdByCommonTraitTypeIdListAsync(int universityId, int commonTraitTypeId);
        Task<List<int>> GetEntityTraitToSchoolIdByCommonTraitTypeIdListAsync(int schoolId, int commonTraitTypeId);
        Task<List<int>> GetEntityTraitToCourseIdByCommonTraitTypeIdListAsync(int courseId, int commonTraitTypeId);
        Task<List<int>> GetEntityTraitToPageIdListAsync(int pageId);
        Task CreateEntityTraitToPageAsync(int pageId, int commonTraitId);
        Task DeleteEntityTraitToPageAsync(int pageId, int commonTraitId);
        Task<List<int>> GetEntityTraitToHousingIdListAsync(int housingId);
        Task CreateEntityTraitToHousingAsync(int housingId, int commonTraitId);
        Task DeleteEntityTraitToHousingAsync(int housingId, int commonTraitId);
        Task<List<int>> GetEntityTraitToHousingAccommodationTypeIdListAsync(int housingAccommodationTypeId);
        Task CreateEntityTraitToHousingAccommodationTypeAsync(int housingAccommodationTypeId, int commonTraitId);
        Task DeleteEntityTraitToHousingAccommodationTypeAsync(int housingAccommodationTypeId, int commonTraitId);
    }
}
