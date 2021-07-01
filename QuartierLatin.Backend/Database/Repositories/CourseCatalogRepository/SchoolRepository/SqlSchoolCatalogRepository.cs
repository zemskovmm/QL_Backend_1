using LinqToDB;
using LinqToDB.Data;
using QuartierLatin.Backend.Models.CourseCatalogModels.SchoolModels;
using QuartierLatin.Backend.Models.Repositories.courseCatalogRepository.SchoolRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Database.Repositories.courseCatalogRepository.SchoolRepository
{
    public class SqlSchoolCatalogRepository : ISchoolCatalogRepository
    {
        private readonly AppDbContextManager _db;

        public SqlSchoolCatalogRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<List<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)>> GetSchoolListAsync()
        {
            return await _db.ExecAsync(async db =>
            {
                var schoolLanguageEntity = await SchoolWithLanguages(db)
                    .ToDictionaryAsync(x => x.Key, x => x.Select(i => i.schoolLanguage));

                return schoolLanguageEntity.Select(school =>
                    (school.Key, school.Value.ToDictionary(schoolLang => schoolLang.LanguageId, schoolLang => schoolLang))).ToList();
            });
        }

        public async Task<int> CreateSchoolAsync(int? foundationYear)
        {
            return await _db.ExecAsync(db => db.InsertWithInt32IdentityAsync(new School
            {
                FoundationYear = foundationYear,
            }));
        }

        public async Task CreateSchoolLanguageListAsync(List<SchoolLanguages> schoolLanguage)
        {
            await _db.ExecAsync(db => db.BulkCopyAsync(schoolLanguage));
        }

        public async Task<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)> GetSchoolByIdAsync(int id)
        {
            return await _db.ExecAsync(async db =>
            {
                var courseLanguageEntity = await SchoolWithLanguages(db)
                    .Where(x => x.Key.Id == id)
                    .ToDictionaryAsync(x => x.Key, x => x.Select(i => i.schoolLanguage));

                var (school, schoolLanguage) = courseLanguageEntity.First();

                return (course: school, schoolLanguage: schoolLanguage.ToDictionary(x => x.LanguageId, x => x));
            });
        }

        public async Task UpdateSchoolByIdAsync(int id, int? foundationYear)
        {
            await _db.ExecAsync(db => db.UpdateAsync(new School
            {
                Id = id,
                FoundationYear = foundationYear
            }));
        }

        public async Task CreateOrUpdateSchoolLanguageByIdAsync(int schoolId, string htmlDescription, int languageId, string name,
            string url)
        {
            await _db.ExecAsync(db => db.InsertOrReplaceAsync(new SchoolLanguages
            {
                SchoolId = schoolId,
                LanguageId = languageId,
                Description = htmlDescription,
                Name = name,
                Url = url
            }));
        }

        public async Task<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)> GetSchoolByUrlWithLanguageAsync(int languageId, string url)
        {
            return await _db.ExecAsync(async db =>
            {
                var schoolLanguageEntity = await SchoolWithLanguages(db)
                    .Where(x => x.Key.Id == x.FirstOrDefault(school => school.schoolLanguage.LanguageId == languageId && school.schoolLanguage.Url == url).school.Id)
                    .ToDictionaryAsync(x => x.Key, x => x.Select(i => i.schoolLanguage));

                var (school, schoolLanguage) = schoolLanguageEntity.First();

                return (school: school, schoolLanguage: schoolLanguage.ToDictionary(x => x.LanguageId, x => x));
            });
        }

        private IQueryable<IGrouping<School, (School school, SchoolLanguages schoolLanguage)>> SchoolWithLanguages(AppDbContext db) =>
            (from school in db.Schools
                join schoolLanguage in db.SchoolLanguages on school.Id equals schoolLanguage.SchoolId
                select (school, schoolLanguage)).GroupBy(x => x.school);
    }
}