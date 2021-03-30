﻿using LinqToDB;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuartierLatin.Backend.Database.Repositories.CatalogRepository
{
    public class SqlCommonTraitRepository : ICommonTraitRepository
    {
        private readonly AppDbContextManager _db;

        public SqlCommonTraitRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<int> CreateCommonTraitAsync(int commonTraitTypeId, JObject names, int iconBlobId, int order)
        {
            return await _db.ExecAsync(db => db.InsertWithInt32IdentityAsync(new CommonTrait
            {
                CommonTraitTypeId = commonTraitTypeId,
                IconBlobId = iconBlobId,
                Names = names.ToString(Formatting.None),
                Order = order
            }));
        }

        public async Task CreateOrUpdateCommonTraitToUniversityAsync(int universityId, int commonTraitId)
        {
            await _db.ExecAsync(db => db.InsertOrReplaceAsync(new CommonTraitsToUniversity
            {
                CommonTraitId = commonTraitId,
                UniversityId = universityId
            }));
        }

        public async Task UpdateCommonTraitAsync(int id, int commonTraitTypeId, JObject names, int iconBlobId, int order)
        {
            await _db.ExecAsync(db => db.UpdateAsync(new CommonTrait
            {
                Id = id,
                CommonTraitTypeId = commonTraitTypeId,
                IconBlobId = iconBlobId,
                Names = names.ToString(Formatting.None),
                Order = order
            }));
        }

        public async Task DeleteCommonTraitAsync(int id)
        {
            await _db.ExecAsync(db => db.CommonTraits.Where(trait => trait.Id == id).DeleteAsync());
        }

        public async Task DeleteCommonTraitToUniversityAsync(int universityId, int commonTraitId)
        {
            await _db.ExecAsync(db => db.CommonTraitsToUniversities.Where(trait =>
                trait.UniversityId == universityId && trait.CommonTraitId == commonTraitId).DeleteAsync());
        }

        public async Task<CommonTrait> GetCommonTraitAsync(int id)
        {
            return await _db.ExecAsync(db => db.CommonTraits.FirstOrDefaultAsync(trait => trait.Id == id));
        }
    }
}
