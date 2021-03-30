using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;

namespace QuartierLatin.Backend.Database.Repositories.CatalogRepository
{
    public class SqlCommonTraitTypeRepository : ICommonTraitTypeRepository
    {
        private readonly AppDbContextManager _db;

        public SqlCommonTraitTypeRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<int> CreateCommonTraitTypeAsync(JObject names, string? identifier)
        {
            return await _db.ExecAsync(db => db.InsertWithInt32IdentityAsync(new CommonTraitType
            {
                Names = names.ToString(Formatting.None),
                Identifier = identifier
            }));
        }

        public async Task UpdateCommonTraitTypeAsync(int id, JObject names, string? identifier)
        {
            await _db.ExecAsync(db => db.UpdateAsync(new CommonTraitType
            {
                Id = id,
                Names = names.ToString(Formatting.None),
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

        public async Task DeleteCommonTraitTypeAsync(int id)
        {
            await _db.ExecAsync(db => db.CommonTraitTypes.Where(trait => trait.Id == id).DeleteAsync());
        }

        public async Task DeleteCommonTraitTypesForEntityAsync(int commonTraitId)
        {
            await _db.ExecAsync(db => db.CommonTraitTypesForEntities.Where(trait => trait.CommonTraitId == commonTraitId).DeleteAsync());
        }

        public async Task<CommonTraitType> GetCommonTraitTypeAsync(int id)
        {
            return await _db.ExecAsync(db => db.CommonTraitTypes.FirstOrDefaultAsync(trait => trait.Id == id));
        }

        public async Task<CommonTraitTypesForEntity> GetCommonTraitTypesForEntityAsync(int commonTraitId)
        {
            return await _db.ExecAsync(db => db.CommonTraitTypesForEntities.FirstOrDefaultAsync(trait => trait.CommonTraitId == commonTraitId));
        }
    }
}
