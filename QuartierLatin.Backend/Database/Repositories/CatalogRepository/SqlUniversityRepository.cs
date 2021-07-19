using LinqToDB;
using LinqToDB.Data;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Database.Repositories.CatalogRepository
{
    public class SqlUniversityRepository : IUniversityRepository
    {
        private readonly AppDbContextManager _db;

        public SqlUniversityRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task CreateOrUpdateUniversityLanguageAsync(int universityId, int languageId, string name,
            string description, string url, JObject? metadata)
        {
            await _db.ExecAsync(
                db => CreateOrUpdateUniversityCore(db, universityId, languageId, name, description, url, metadata));
        }

        public async Task<List<int>> GetUniversityIdListAsync()
        {
            return await _db.ExecAsync(db => db.Universities.Select(university => university.Id).ToListAsync());
        }

        public async Task<Dictionary<int, UniversityLanguage>> GetUniversityLanguageByUniversityIdAsync(
            int universityId)
        {
            return await _db.ExecAsync(db => db.UniversityLanguages
                .Where(universityLanguage => universityLanguage.UniversityId == universityId)
                .ToDictionaryAsync(universityLanguage => universityLanguage.LanguageId,
                    universityLanguage => universityLanguage));
        }

        public async Task<University> GetUniversityByIdAsync(int id)
        {
            return await _db.ExecAsync(db => db.Universities.FirstOrDefaultAsync(university => university.Id == id));
        }

        public async Task CreateUniversityLanguageListAsync(List<UniversityLanguage> universityLanguage)
        {
            await _db.ExecAsync(db => db.BulkCopyAsync(universityLanguage));
        }

        public async Task<int> CreateUniversityAsync(int? foundationYear, int? logoId, int? bannerId)
        {
            return await _db.ExecAsync(db => db.InsertWithInt32IdentityAsync(new University
            {
                FoundationYear = foundationYear,
                LogoId = logoId,
                BannerId = bannerId
            }));
        }

        public async Task UpdateUniversityAsync(int id, int? foundationYear, int? logoId, int? bannerId)
        {
            await _db.ExecAsync(db => db.UpdateAsync(new University
            {
                Id = id,
                FoundationYear = foundationYear,
                LogoId = logoId,
                BannerId = bannerId
            }));
        }

        public async Task<int> GetUniversityIdByUrl(string url)
        {
            return await _db.ExecAsync(db =>
                db.UniversityLanguages.Where(university => university.Url == url).Select(university => university.UniversityId).FirstAsync());
        }

        public Task<List<Specialty>> GetSpecialtiesUniversityByUniversityIdList(int universityId) => _db.Exec(db =>
            (from map in db.UniversitySpecialties.Where(x => x.UniversityId == universityId)
                join spec in db.Specialties on map.SpecialtyId equals spec.Id
                select spec).ToListAsync());

        public async Task<Specialty> GetSpecialtyById(int specialtyId)
        {
            return await _db.ExecAsync(db => db.Specialties.FirstOrDefaultAsync(specialty => specialty.Id == specialtyId));
        }

        public async Task<int> GetUniversityIdByUrlAndLanguage(int languageId, string url)
        {
            return await _db.ExecAsync(db =>
                db.UniversityLanguages.Where(university => university.Url == url && university.LanguageId == languageId)
                    .Select(university => university.UniversityId).FirstAsync());
        }

        public async Task<(int totalItems, List<(University university, UniversityLanguage universityLanguage, int cost)>)> GetUniversityPageByFilter(List<List<int>> commonTraitGroups,
            List<int> specialtyCategoriesId, List<int> degreeIds, List<int> priceIds, int languageId, int skip, int take)
        {
            return await _db.ExecAsync(async db =>
            {
                var universities = db.Universities.AsQueryable();
                if (commonTraitGroups.Any())
                {
                    var universitiesWithTraits = db.CommonTraitsToUniversities.AsQueryable();
                    foreach (var commonTraitGroup in commonTraitGroups)
                    {
                        if (commonTraitGroup.Count != 0)
                            universitiesWithTraits =
                                universitiesWithTraits.Where(t => commonTraitGroup.Contains(t.CommonTraitId));
                    }

                    universities = universities.Where(uni =>
                        universitiesWithTraits.Select(x => x.UniversityId).Contains(uni.Id));
                }

                if (specialtyCategoriesId.Any())
                {
                    var universitySpecialties = db.UniversitySpecialties.AsQueryable();


                    var matchingSpecialties = db.Specialties
                        .Where(spec => specialtyCategoriesId.Contains(spec.CategoryId)).Select(x => x.Id);
                    universitySpecialties =
                        universitySpecialties.Where(spec => matchingSpecialties.Contains(spec.SpecialtyId));


                    universities = universities.Where(uni =>
                        universitySpecialties.Select(x => x.UniversityId).Contains(uni.Id));
                }

                IQueryable<UniversityDegree> degreeQueryable = db.UniversityDegrees;
                
                
                if (degreeIds.Any()) 
                    degreeQueryable = degreeQueryable.Where(d => degreeIds.Contains(d.DegreeId));
                
                if(priceIds.Any())
                    degreeQueryable = degreeQueryable.Where(d => priceIds.Contains(d.CostGroup));

                if (priceIds.Any() || degreeIds.Any())
                    universities =
                        universities.Where(uni => degreeQueryable.Select(x => x.UniversityId).Contains(uni.Id));

                var universitiesWithLanguages = from uni in universities
                    join lang in db.UniversityLanguages.Where(l => l.LanguageId == languageId)
                        on uni.Id equals lang.UniversityId
                    select new
                    {
                        uni,
                        lang,
                        costGroup = degreeQueryable.Where(d => d.UniversityId == uni.Id)
                            .Min(d => d.CostGroup)
                    };

                var totalCount = await universitiesWithLanguages.CountAsync();

                return (totalCount,
                    (await universitiesWithLanguages.OrderBy(x => x.uni.Id).Skip(skip).Take(take).ToListAsync())
                    .Select(x => (university: x.uni, universityLanguage: x.lang, cost: x.costGroup)).ToList());
            });
        }

        private static async Task CreateOrUpdateUniversityCore(AppDbContext db, int universityId, int languageId,
            string name, string description, string url, JObject? metadata)
        {
            await db.InsertOrReplaceAsync(new UniversityLanguage
            {
                UniversityId = universityId,
                LanguageId = languageId,
                Name = name,
                Description = description,
                Url = url,
                Metadata = metadata?.ToString()
            });
        }
    }
}