using LinqToDB;
using LinqToDB.Data;
using QuartierLatin.Backend.Models.CatalogModels;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
            string description, string url)
        {
            await _db.ExecAsync(
                db => CreateOrUpdateUniversityCore(db, universityId, languageId, name, description, url));
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

        public async Task<int> CreateUniversityAsync(int? foundationYear, string website)
        {
            return await _db.ExecAsync(db => db.InsertWithInt32IdentityAsync(new University
            {
                FoundationYear = foundationYear,
                Website = website
            }));
        }

        public async Task UpdateUniversityAsync(int id, int? foundationYear, string website)
        {
            await _db.ExecAsync(db => db.UpdateAsync(new University
            {
                Id = id,
                FoundationYear = foundationYear,
                Website = website
            }));
        }

        public async Task<int> GetUniversityIdByUrl(string url)
        {
            return await _db.ExecAsync(db =>
                db.UniversityLanguages.Where(university => university.Url == url).Select(university => university.UniversityId).FirstAsync());
        }

        public async Task<List<(Specialty, int)>> GetSpecialtiesUniversityByUniversityIdList(int universityId)
        {
            var response = new List<(Specialty, int)>();

            var universitySpecialty = await _db.ExecAsync(db =>
                db.UniversitySpecialties.Where(speciallty => speciallty.UniversityId == universityId).ToListAsync());

            foreach (var specialty in universitySpecialty)
            {
                var specialtyEntity = await GetSpecialtyById(specialty.SpecialtyId);
                response.Add((specialtyEntity, specialty.CostTo));
            }

            return response;
        }

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

        public async Task<(int totalPages, List<(University, UniversityLanguage, int cost)>)> GetUniversityPageByFilter(List<List<int>> commonTraitGroups, List<int> specialtyCategoriesId, List<int> priceIds, int languageId, int skip, int take)
        {
            var pageSize = take;

            return await _db.ExecAsync(async db =>
            {
                var universities = db.Universities.AsQueryable();
                if (commonTraitGroups.Any())
                {
                    var universitiesWithTraits = db.CommonTraitsToUniversities.AsQueryable();
                    foreach (var commonTraitGroup in commonTraitGroups)
                    {
                        if (commonTraitGroup.Count != 0)
                            universitiesWithTraits = universitiesWithTraits.Where(t => commonTraitGroup.Contains(t.CommonTraitId));
                    }

                    if (specialtyCategoriesId.Any())
                    {
                        var universitySpecialtyTable = db.UniversitySpecialties.AsQueryable();
                        var specialtyTable = db.Specialties.Where(specialtyEntity =>
                            specialtyCategoriesId.Contains(specialtyEntity.CategoryId)).AsQueryable();

                        var universitiesId = from universitySpecialty in universitySpecialtyTable
                                             join specialty in specialtyTable on universitySpecialty.SpecialtyId equals specialty.Id
                            select universitySpecialty.UniversityId;

                        var test = universitiesId.ToList();

                        universitiesWithTraits = universitiesWithTraits.Where(t => universitiesId.Contains(t.UniversityId));

                        if (priceIds.Any())
                        {
                            var universitySpecialtyTableOrBuilder = new OrBuilder<UniversitySpecialty>(universitySpecialtyTable);
                            foreach (var priceId in priceIds)
                            {
                                var _ = priceId switch
                                {
                                    1 => universitySpecialtyTableOrBuilder.Or(universitySpecialty => universitySpecialty.CostTo <= 10000),
                                    2 => universitySpecialtyTableOrBuilder.Or(universitySpecialty => universitySpecialty.CostTo <= 20000),
                                    3 => universitySpecialtyTableOrBuilder.Or(universitySpecialty => universitySpecialty.CostTo <= 30000),
                                    _ => throw new System.NotImplementedException(),
                                };
                            }

                            universitySpecialtyTable = universitySpecialtyTableOrBuilder.GetWhereQueryable();

                            universitiesId = from universitySpecialty in universitySpecialtyTable
                                             join specialty in specialtyTable on universitySpecialty.SpecialtyId equals specialty.Id
                                select universitySpecialty.UniversityId;

                            var test1 = universitiesId.ToList();

                            universitiesWithTraits = universitiesWithTraits.Where(t => universitiesId.Contains(t.UniversityId));
                        }
                    }

                    var universityIdsWithTraits = universitiesWithTraits.Select(x => x.UniversityId);
                    universities = universities.Where(u => universityIdsWithTraits.Contains(u.Id));
                }

                var universitiesWithLanguages = from uni in universities
                    join lang in db.UniversityLanguages on uni.Id equals lang.UniversityId
                    join uniSpecialty in db.UniversitySpecialties on uni.Id equals uniSpecialty.UniversityId
                    where lang.LanguageId == languageId || lang.LanguageId == db.UniversityLanguages.FirstOrDefault(lang => lang.UniversityId == uni.Id).LanguageId
                    select new {uni, lang, uniSpecialty.CostFrom, uniSpecialty.CostTo};

                var totalCount = await universitiesWithLanguages.CountAsync();

                var pageCount = FilterHelper.PageCount(totalCount, pageSize);

                return (pageCount,
                    (await universitiesWithLanguages.OrderBy(x => x.uni.Id).Skip(skip).Take(take).ToListAsync())
                    .Select(x => (x.uni, x.lang, x.CostFrom)).ToList());
            });
        }

        private static async Task CreateOrUpdateUniversityCore(AppDbContext db, int universityId, int languageId,
            string name, string description, string url)
        {
            await db.InsertOrReplaceAsync(new UniversityLanguage
            {
                UniversityId = universityId,
                LanguageId = languageId,
                Name = name,
                Description = description,
                Url = url
            });
        }
    }
}