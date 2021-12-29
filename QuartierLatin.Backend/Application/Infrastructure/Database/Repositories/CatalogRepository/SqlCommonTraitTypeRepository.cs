using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Application.ApplicationCore.Models;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CatalogModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CourseCatalogModels.CoursesModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CourseCatalogModels.SchoolModels;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Enums;
using QuartierLatin.Backend.Application.ApplicationCore.Models.HousingModels;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Repositories.CatalogRepository
{
    public class SqlCommonTraitTypeRepository : ICommonTraitTypeRepository
    {
        private readonly AppDbContextManager _db;

        public SqlCommonTraitTypeRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<int> CreateCommonTraitTypeAsync(Dictionary<string, string> names, string? identifier, int order)
        {
            return await _db.ExecAsync(db => db.InsertWithInt32IdentityAsync(new CommonTraitType
            {
                Names = names,
                Identifier = identifier,
                Order = order
            }));
        }

        public async Task UpdateCommonTraitTypeAsync(int id, Dictionary<string, string> names, string? identifier, int order)
        {
            await _db.ExecAsync(db => db.UpdateAsync(new CommonTraitType
            {
                Id = id,
                Names = names,
                Identifier = identifier,
                Order = order
            }));
        }
		

        public async Task CreateOrUpdateCommonTraitTypesForEntityAsync(int commonTraitId, EntityType entityType)
        {
            await _db.ExecAsync(db => db.InsertAsync(new CommonTraitTypesForEntity
            {
                CommonTraitId = commonTraitId,
                EntityType = entityType
            }));
        }
		
		
		public async Task<List<CommonTraitTypesForEntity>> GetEntityTypesTraitTypeByIdAsync(int id){
            return await _db.ExecAsync(db =>
                db.CommonTraitTypesForEntities.Where(trait => trait.CommonTraitId == id).ToListAsync());			
			
		}

        public async Task DeleteAllEntityTypesForTrait(int commonTraitId)
        {
            await _db.ExecAsync(db =>
                db.CommonTraitTypesForEntities
                    .Where(trait => trait.CommonTraitId == commonTraitId)
                    .DeleteAsync());
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
			//	join commonTrait in db.CommonTraits.Where(c => c.CommonTraitTypeId == traitType.Id) on  commonTrait.CommonTraitTypeId equals traitType.Id
                select traitType  ).Distinct().ToListAsync());

        public async Task<List<int>> GetEntityTraitToSchoolIdListAsync(int schoolId)
        {
            return await _db.ExecAsync(db =>
                db.CommonTraitToSchools.Where(trait => trait.SchoolId == schoolId)
                    .Select(trait => trait.CommonTraitId).ToListAsync());
        }

        public async Task CreateEntityTraitToSchoolAsync(int schoolId, int commonTraitId)
        {
            await _db.ExecAsync(db => db.InsertAsync(new CommonTraitToSchool()
            {
                CommonTraitId = commonTraitId,
                SchoolId = schoolId
            }));
        }

        public async Task DeleteEntityTraitToSchoolAsync(int schoolId, int commonTraitId)
        {
            await _db.ExecAsync(db =>
                db.CommonTraitToSchools
                    .Where(trait => trait.SchoolId == schoolId && trait.CommonTraitId == commonTraitId)
                    .DeleteAsync());
        }

        public async Task<List<int>> GetEntityTraitToCourseIdListAsync(int courseId)
        {
            return await _db.ExecAsync(db =>
                db.CommonTraitToCourses.Where(trait => trait.CourseId == courseId)
                    .Select(trait => trait.CommonTraitId).ToListAsync());
        }

        public async Task CreateEntityTraitToCourseAsync(int courseId, int commonTraitId)
        {
            await _db.ExecAsync(db => db.InsertAsync(new CommonTraitToCourse()
            {
                CommonTraitId = commonTraitId,
                CourseId = courseId
            }));
        }

        public async Task DeleteEntityTraitToCourseAsync(int courseId, int commonTraitId)
        {
            await _db.ExecAsync(db =>
                db.CommonTraitToCourses
                    .Where(trait => trait.CourseId == courseId && trait.CommonTraitId == commonTraitId)
                    .DeleteAsync());
        }

        public async Task<List<int>> GetEntityTraitToUniversityIdByCommonTraitTypeIdListAsync(int universityId,
            int commonTraitTypeId) => await 
            _db.ExecAsync(db => (from universityTrait in db.CommonTraitsToUniversities.Where(university =>
                        university.UniversityId == universityId)
                    join commonTrait in db.CommonTraits on universityTrait.CommonTraitId equals commonTrait.Id
                    where commonTrait.CommonTraitTypeId == commonTraitTypeId
                    select commonTrait.Id).ToListAsync());

        public async Task<List<int>> GetEntityTraitToSchoolIdByCommonTraitTypeIdListAsync(int schoolId, int commonTraitTypeId) => await
            _db.ExecAsync(db => (from schoolTrait in db.CommonTraitToSchools.Where(school =>
                    school.SchoolId == schoolId)
                join commonTrait in db.CommonTraits on schoolTrait.CommonTraitId equals commonTrait.Id
                where commonTrait.CommonTraitTypeId == commonTraitTypeId
                select commonTrait.Id).ToListAsync());

        public async Task<List<int>> GetEntityTraitToCourseIdByCommonTraitTypeIdListAsync(int courseId, int commonTraitTypeId) => await
            _db.ExecAsync(db => (from courseTrait in db.CommonTraitToCourses.Where(course =>
                    course.CourseId == courseId)
                join commonTrait in db.CommonTraits on courseTrait.CommonTraitId equals commonTrait.Id
                where commonTrait.CommonTraitTypeId == commonTraitTypeId
                select commonTrait.Id).ToListAsync());
                
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

        public async Task<List<int>> GetEntityTraitToHousingIdListAsync(int housingId)
        {
            return await _db.ExecAsync(db =>
                db.CommonTraitToHousings.Where(trait => trait.HousingId == housingId)
                    .Select(trait => trait.CommonTraitId).ToListAsync());
        }

        public async Task CreateEntityTraitToHousingAsync(int housingId, int commonTraitId)
        {
            await _db.ExecAsync(db => db.InsertAsync(new CommonTraitToHousing
            {
                CommonTraitId = commonTraitId,
                HousingId = housingId
            }));
        }

        public async Task DeleteEntityTraitToHousingAsync(int housingId, int commonTraitId)
        {
            await _db.ExecAsync(db =>
                db.CommonTraitToHousings
                    .Where(trait => trait.HousingId == housingId && trait.CommonTraitId == commonTraitId)
                    .DeleteAsync());
        }

        public async Task<List<int>> GetEntityTraitToHousingAccommodationTypeIdListAsync(int housingAccommodationTypeId)
        {
            return await _db.ExecAsync(db =>
                db.CommonTraitToHousingAccommodationTypes.Where(trait => trait.HousingAccommodationTypeId == housingAccommodationTypeId)
                    .Select(trait => trait.CommonTraitId).ToListAsync());
        }

        public async Task CreateEntityTraitToHousingAccommodationTypeAsync(int housingAccommodationTypeId, int commonTraitId)
        {
            await _db.ExecAsync(db => db.InsertAsync(new CommonTraitToHousingAccommodationType
            {
                CommonTraitId = commonTraitId,
                HousingAccommodationTypeId = housingAccommodationTypeId
            }));
        }

        public async Task DeleteEntityTraitToHousingAccommodationTypeAsync(int housingAccommodationTypeId, int commonTraitId)
        {
            await _db.ExecAsync(db =>
                db.CommonTraitToHousingAccommodationTypes
                    .Where(trait => trait.HousingAccommodationTypeId == housingAccommodationTypeId && trait.CommonTraitId == commonTraitId)
                    .DeleteAsync());
        }
    }
}