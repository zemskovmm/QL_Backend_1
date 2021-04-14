﻿using QuartierLatin.Backend.Models.CatalogModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Models.Repositories.CatalogRepositoies
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
        Task<List<CommonTrait>> GetCommonTraitListByTypeIdAndUniversityId(int typeId, int universityId);
    }
}