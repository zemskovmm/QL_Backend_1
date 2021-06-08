using LinqToDB;
using LinqToDB.Data;
using QuartierLatin.Backend.Models.CurseCatalogModels.SchoolModels;
using QuartierLatin.Backend.Models.Repositories.CurseCatalogRepository.SchoolRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Database.Repositories.CurseCatalogRepository.SchoolRepository
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
                var schools = await db.Schools.Select(school => GetSchoolByIdAsync(school.Id)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult()).ToListAsync();

                return schools;
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
                var school = await db.Schools.FirstOrDefaultAsync(school => school.Id == id);

                var schoolLanguage = await db.SchoolLanguages.Where(schoolLang => schoolLang.SchoolId == school.Id)
                    .ToDictionaryAsync(schoolLang => schoolLang.LanguageId, schoolLang => schoolLang);

                return (school: school, schoolLanguage: schoolLanguage);
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
    }
}