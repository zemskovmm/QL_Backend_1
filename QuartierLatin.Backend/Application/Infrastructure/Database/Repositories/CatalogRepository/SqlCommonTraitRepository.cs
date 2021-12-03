using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.HousingModels;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Repositories.CatalogRepository
{
    public class SqlCommonTraitRepository : ICommonTraitRepository
    {
        private readonly AppDbContextManager _db;

        public SqlCommonTraitRepository(AppDbContextManager db)
        {
            _db = db;
        }
		
		
		
		 public async Task<List<CommonTrait>> GetCommonTraitListByParentId(int parentId){
            return await _db.ExecAsync(db =>
                db.CommonTraits.Where(trait => trait.ParentId == parentId).ToListAsync());			 
			 
		 }
		 
		 public async Task<int[]> getHousingTraitIds(){
			            var traits_for_housong_array = await _db.ExecAsync(db =>
                db.CommonTraitToHousing.Where(x => x.CommonTraitId != null).ToListAsync());		
				
			var traits_array = await _db.ExecAsync(db =>
                db.CommonTraits.Where(x => (traits_for_housong_array.Select(x => x.CommonTraitId)).Contains(x.Id)).ToListAsync());
            int?[] parent_trait_ids=traits_array.Select(x => x.ParentId).ToArray();	
			int[] traits_ids_array=traits_array.Select(x =>x.Id).ToArray();

			
			
			List<int> termsList = new List<int>();
			foreach (var curParentId in parent_trait_ids){
				termsList.Add(curParentId.GetValueOrDefault());
			}
			foreach (var curTraitId in traits_ids_array){
				termsList.Add(curTraitId);
			}			


				return termsList.ToArray();
				

 
			 
		
			 
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
		
        public async Task<List<CommonTrait>> GetCommonTraitListByTypeIdWithoutParent(int typeId)
        {
            return await _db.ExecAsync(db =>
                db.CommonTraits.Where(trait => trait.CommonTraitTypeId == typeId && trait.ParentId == null).ToListAsync());
        }	

        public async Task<List<CommonTrait>> GetCommonTraitListByTypeIdByParentId(int typeId,int parentId)
        {
            return await _db.ExecAsync(db =>
                db.CommonTraits.Where(trait => trait.CommonTraitTypeId == typeId && trait.ParentId==parentId).ToListAsync());
        }			

        public Task<List<CommonTrait>> GetCommonTraitListByTypeIds(int[] typeIds) => _db.ExecAsync(db =>
            db.CommonTraits.Where(trait => typeIds.Contains(trait.CommonTraitTypeId) && trait.ParentId == null).ToListAsync());

        public async Task<List<CommonTrait>> GetCommonTraitListByTypeIdAndUniversityId(int typeId, int universityId)
        {
            return await _db.ExecAsync(db =>
                (from courseTraits in db.CommonTraitsToUniversities.Where(trait => trait.UniversityId == universityId)
                    join trait in db.CommonTraits.Where(trait => trait.CommonTraitTypeId == typeId) on courseTraits
                        .CommonTraitId equals trait.Id
                    select trait).ToListAsync()
            );
        }

        public Task<Dictionary<int, List<CommonTrait>>> GetCommonTraitListByUniversityIds(IEnumerable<int> ids) =>
            _db.ExecAsync(async db =>
                (await (from mapping in db.CommonTraitsToUniversities.Where(x => ids.Contains(x.UniversityId))
                    join trait in db.CommonTraits on mapping.CommonTraitId equals trait.Id
                    select new {mapping, trait}).ToListAsync())
                .GroupBy(x => x.mapping.UniversityId)
                .ToDictionary(x => x.Key, x => x.Select(t => t.trait).ToList()));

        public Task<Dictionary<int, List<CommonTrait>>> GetCommonTraitListByPageIds(IEnumerable<int> ids) =>
            _db.ExecAsync(async db =>
                (await (from mapping in db.CommonTraitsToPages.Where(x => ids.Contains(x.PageId))
                    join trait in db.CommonTraits on mapping.CommonTraitId equals trait.Id
                    select new { mapping, trait }).ToListAsync())
                .GroupBy(x => x.mapping.PageId)
                .ToDictionary(x => x.Key, x => x.Select(t => t.trait).ToList()));

        public async Task<List<CommonTrait>> GetCommonTraitListByTypeIdAndSchoolIdAsync(int traitTypeId, int schoolId)
        {
            return await _db.ExecAsync(db =>
                (from courseTraits in db.CommonTraitToSchools.Where(trait => trait.SchoolId == schoolId)
                    join trait in db.CommonTraits.Where(trait => trait.CommonTraitTypeId == traitTypeId) on courseTraits
                        .CommonTraitId equals trait.Id
                    select trait).ToListAsync()
            );
        }

        public async Task<List<CommonTrait>> GetTraitOfTypesByTypeIdAndCourseIdAsync(int traitTypeId, int courseId)
        {
            return await _db.ExecAsync(db =>
                (from courseTraits in db.CommonTraitToCourses.Where(trait => trait.CourseId == courseId)
                    join trait in db.CommonTraits.Where(trait => trait.CommonTraitTypeId == traitTypeId) on courseTraits
                        .CommonTraitId equals trait.Id
                    select trait).ToListAsync()
            );
        }

        public async Task<List<CommonTrait>> GetTraitOfTypesByIdentifierAsync(string traitIdentifier)
        {
            return await _db.ExecAsync(async db =>
            {
               var traitTypes = db.CommonTraitTypes.AsEnumerable();

               var traitTypeId = traitTypes.FirstOrDefault(traitType => traitType.Identifier == traitIdentifier).Id;

               return await GetCommonTraitListByTypeId(traitTypeId);
            });
        }

        public Task<Dictionary<int, List<CommonTrait>>> GetCommonTraitListByCourseIdsAsync(IEnumerable<int> courseIds) =>
            _db.ExecAsync(async db =>
                (await (from mapping in db.CommonTraitToCourses.Where(x => courseIds.Contains(x.CourseId))
                    join trait in db.CommonTraits on mapping.CommonTraitId equals trait.Id
                    select new { mapping, trait }).ToListAsync())
                .GroupBy(x => x.mapping.CourseId)
                .ToDictionary(x => x.Key, x => x.Select(t => t.trait).ToList()));

        public async Task<List<CommonTrait>> GetTraitOfTypesByTypeIdAndHousingIdAsync(int traitTypeId, int housingId)
        {
            return await _db.ExecAsync(db =>
                (from courseTraits in db.CommonTraitToHousings.Where(trait => trait.HousingId == housingId)
                    join trait in db.CommonTraits.Where(trait => trait.CommonTraitTypeId == traitTypeId) on courseTraits
                        .CommonTraitId equals trait.Id
                    select trait).ToListAsync()
            );
        }

        public Task<Dictionary<int, List<CommonTrait>>> GetCommonTraitListByHousingIdsAsync(IEnumerable<int> housingIds) =>
            _db.ExecAsync(async db =>
                (await (from mapping in db.CommonTraitToHousings.Where(x => housingIds.Contains(x.HousingId))
                    join trait in db.CommonTraits on mapping.CommonTraitId equals trait.Id
                    select new { mapping, trait }).ToListAsync())
                .GroupBy(x => x.mapping.HousingId)
                .ToDictionary(x => x.Key, x => x.Select(t => t.trait).ToList()));

        public Task<Dictionary<int, List<CommonTrait>>> GetCommonTraitListByHousingAccommodationTypeIdsAsync(IEnumerable<int> housingAccommodationIds) =>
            _db.ExecAsync(async db =>
                (await (from mapping in db.CommonTraitToHousingAccommodationTypes.Where(x => housingAccommodationIds.Contains(x.HousingAccommodationTypeId))
                    join trait in db.CommonTraits on mapping.CommonTraitId equals trait.Id
                    select new { mapping, trait }).ToListAsync())
                .GroupBy(x => x.mapping.HousingAccommodationTypeId)
                .ToDictionary(x => x.Key, x => x.Select(t => t.trait).ToList()));
    }
}