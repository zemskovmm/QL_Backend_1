﻿using LinqToDB;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Database.Repositories.CatalogRepository
{
    public class SqlCommonTraitRepository : ICommonTraitRepository
    {
        private readonly AppDbContextManager _db;

        public SqlCommonTraitRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<int> CreateCommonTraitAsync(int commonTraitTypeId, Dictionary<string, string> names, int? iconBlobId, int order, int? parentId)
        {
            return await _db.ExecAsync(db => db.InsertWithInt32IdentityAsync(new CommonTrait
            {
                CommonTraitTypeId = commonTraitTypeId,
                IconBlobId = iconBlobId,
                Names = names,
                Order = order,
                ParentId = parentId
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

        public async Task UpdateCommonTraitAsync(int id, int commonTraitTypeId, Dictionary<string, string> names, int? iconBlobId,
            int order, int? parentId)
        {
            await _db.ExecAsync(db => db.UpdateAsync(new CommonTrait
            {
                Id = id,
                CommonTraitTypeId = commonTraitTypeId,
                IconBlobId = iconBlobId,
                Names = names,
                Order = order,
                ParentId = parentId
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

        public async Task<List<CommonTrait>> GetCommonTraitListByTypeId(int typeId)
        {
            return await _db.ExecAsync(db =>
                db.CommonTraits.Where(trait => trait.CommonTraitTypeId == typeId).ToListAsync());
        }

        public Task<List<CommonTrait>> GetCommonTraitListByTypeIds(int[] typeIds) => _db.ExecAsync(db =>
            db.CommonTraits.Where(trait => typeIds.Contains(trait.CommonTraitTypeId)).ToListAsync());

        public async Task<List<CommonTrait>> GetCommonTraitListByTypeIdAndUniversityId(int typeId, int universityId)
        {
            var universityTraitsId = await _db.ExecAsync(db =>
                db.CommonTraitsToUniversities.Where(trait => trait.UniversityId == universityId)
                    .Select(trait => trait.CommonTraitId)
                    .ToListAsync());

            return await _db.ExecAsync(db =>
                db.CommonTraits
                    .Where(trait => universityTraitsId.Contains(trait.Id) && trait.CommonTraitTypeId == typeId)
                    .ToListAsync());
        }

        public Task<Dictionary<int, List<CommonTrait>>> GetCommonTraitListByUniversityIds(IEnumerable<int> ids) =>
            _db.ExecAsync(async db =>
                (await (from mapping in db.CommonTraitsToUniversities.Where(x => ids.Contains(x.UniversityId))
                    join trait in db.CommonTraits on mapping.CommonTraitId equals trait.Id
                    select new {mapping, trait}).ToListAsync())
                .GroupBy(x => x.mapping.UniversityId)
                .ToDictionary(x => x.Key, x => x.Select(t => t.trait).ToList()));

        public async Task<List<CommonTrait>> GetCommonTraitListByTypeIdAndSchoolIdAsync(int traitTypeId, int schoolId)
        {
            var schoolTraitsId = await _db.ExecAsync(db =>
                db.CommonTraitToSchools.Where(trait => trait.SchoolId == schoolId)
                    .Select(trait => trait.CommonTraitId)
                    .ToListAsync());

            return await _db.ExecAsync(db =>
                db.CommonTraits
                    .Where(trait => schoolTraitsId.Contains(trait.Id) && trait.CommonTraitTypeId == traitTypeId)
                    .ToListAsync());
        }

        public async Task<List<CommonTrait>> GetTraitOfTypesByTypeIdAndCurseIdAsync(int traitTypeId, int curseId)
        {
            var curseTraitsId = await _db.ExecAsync(db =>
                db.CommonTraitToCurses.Where(trait => trait.CurseId == curseId)
                    .Select(trait => trait.CommonTraitId)
                    .ToListAsync());

            return await _db.ExecAsync(db =>
                db.CommonTraits
                    .Where(trait => curseTraitsId.Contains(trait.Id) && trait.CommonTraitTypeId == traitTypeId)
                    .ToListAsync());
        }
    }
}