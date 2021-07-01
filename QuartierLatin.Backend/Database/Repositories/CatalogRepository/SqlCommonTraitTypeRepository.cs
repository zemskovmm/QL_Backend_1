using LinqToDB;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Models;

namespace QuartierLatin.Backend.Database.Repositories.CatalogRepository
{
    public class SqlCommonTraitTypeRepository : ICommonTraitTypeRepository
    {
        private readonly AppDbContextManager _db;

        public SqlCommonTraitTypeRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<int> CreateCommonTraitTypeAsync(Dictionary<string, string> names, string? identifier)
        {
            return await _db.ExecAsync(db => db.InsertWithInt32IdentityAsync(new CommonTraitType
            {
                Names = names,
                Identifier = identifier
            }));
        }

        public async Task UpdateCommonTraitTypeAsync(int id, Dictionary<string, string> names, string? identifier)
        {
            await _db.ExecAsync(db => db.UpdateAsync(new CommonTraitType
            {
                Id = id,
                Names = names,
                Identifier = identifier
            }));
        }

        public async Task CreateOrUpdateCommonTraitTypesForEntityAsync(int commonTraitId, EntityType entityType)
        {
            await _db.ExecAsync(db => db.InsertOrReplaceAsync(new CommonTraitTypesForEntity
            {
                CommonTraitId = commonTraitId,
                EntityType = entityType
            }));
        }

        public async Task DeleteCommonTraitTypesForEntityAsync(int commonTraitId, EntityType entityType)
        {
            await _db.ExecAsync(db =>
                db.CommonTraitTypesForEntities
                    .Where(trait => trait.CommonTraitId == commonTraitId && trait.EntityType == entityType)
                    .DeleteAsync());
        }

        public async Task<CommonTraitType> GetCommonTraitTypeAsync(int id)
        {
            return await _db.ExecAsync(db => db.CommonTraitTypes.FirstOrDefaultAsync(trait => trait.Id == id));
        }

        public async Task<List<CommonTraitType>> GetCommonTraitTypeListAsync()
        {
            return await _db.ExecAsync(db => db.CommonTraitTypes.ToListAsync());
        }

        public async Task<IEnumerable<int>> GetTraitTypeForEntitiesByEntityTypeIdListAsync(EntityType entityType)
        {
            return await _db.ExecAsync(db =>
                db.CommonTraitTypesForEntities.Where(trait => trait.EntityType == entityType)
                    .Select(trait => trait.CommonTraitId).ToListAsync());
        }

        public async Task<List<int>> GetEntityTraitToUniversityIdListAsync(int universityId)
        {
            return await _db.ExecAsync(db =>
                db.CommonTraitsToUniversities.Where(trait => trait.UniversityId == universityId)
                    .Select(trait => trait.CommonTraitId).ToListAsync());
        }

        public async Task CreateEntityTraitToUniversityAsync(int universityId, int commonTraitId)
        {
            await _db.ExecAsync(db => db.InsertAsync(new CommonTraitsToUniversity
            {
                CommonTraitId = commonTraitId,
                UniversityId = universityId
            }));
        }

        public async Task DeleteEntityTraitToUniversityAsync(int universityId, int commonTraitId)
        {
            await _db.ExecAsync(db =>
                db.CommonTraitsToUniversities
                    .Where(trait => trait.UniversityId == universityId && trait.CommonTraitId == commonTraitId)
                    .DeleteAsync());
        }

        public async Task<List<CommonTraitType>> GetTraitTypesWithIndetifierAsync()
        {
            return await _db.ExecAsync(db => db.CommonTraitTypes.Where(trait => trait.Identifier != null).ToListAsync());
        }

        public Task<List<CommonTraitType>> GetTraitTypesWithIndetifierByEntityTypeAsync(EntityType entityType) =>
            _db.ExecAsync(db => (from traitType in db.CommonTraitTypes
                join traitToEntity in db.CommonTraitTypesForEntities.Where(e => e.EntityType == entityType)
                    on traitType.Id equals traitToEntity.CommonTraitId
                select traitType).Distinct().ToListAsync());

        public async Task<List<int>> GetEntityTraitToPageIdListAsync(int pageId)
        {
            return await _db.ExecAsync(db =>
                db.CommonTraitsToPages.Where(trait => trait.PageId == pageId)
                    .Select(trait => trait.CommonTraitId).ToListAsync());
        }

        public async Task CreateEntityTraitToPageAsync(int pageId, int commonTraitId)
        {
            await _db.ExecAsync(db => db.InsertAsync(new CommonTraitsToPage
            {
                CommonTraitId = commonTraitId,
                PageId = pageId
            }));
        }

        public async Task DeleteEntityTraitToPageAsync(int pageId, int commonTraitId)
        {
            await _db.ExecAsync(db =>
                db.CommonTraitsToPages
                    .Where(trait => trait.PageId == pageId && trait.CommonTraitId == commonTraitId)
                    .DeleteAsync());
        }
    }
}