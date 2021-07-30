using System;
using LinqToDB;
using LinqToDB.Data;
using QuartierLatin.Backend.Models.CourseCatalogModels.SchoolModels;
using QuartierLatin.Backend.Models.Repositories.courseCatalogRepository.SchoolRepository;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Database.Repositories.courseCatalogRepository.SchoolRepository
{
    public class SqlSchoolCatalogRepository : ISchoolCatalogRepository
    {
        private readonly AppDbContextManager _db;
        private (School school, Dictionary<int, SchoolLanguages> schoolLanguage) _test3 { get; set; }
        public SqlSchoolCatalogRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<List<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)>> GetSchoolListAsync()
        {
            return await _db.ExecAsync(async db =>
            {
                var entity = SchoolWithLanguages(db).ToList();

                var response = entity.Select(resp => (resp.School, resp.SchoolLanguage)).ToList();

                return response;
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
                var entity = SchoolWithLanguages(db, school => school.Id == id).First();

                return (school: entity.School, schoolLanguage: entity.SchoolLanguage);
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
            string url, JObject? metadata)
        {
            await _db.ExecAsync(db => db.InsertOrReplaceAsync(new SchoolLanguages
            {
                SchoolId = schoolId,
                LanguageId = languageId,
                Description = htmlDescription,
                Name = name,
                Url = url,
                Metadata = metadata?.ToString()
            }));
        }

        public async Task<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)> GetSchoolByUrlWithLanguageAsync(int languageId, string url)
        {
            return await _db.ExecAsync(async db =>
            {
                var entity = SchoolWithLanguages(db,
                    schoolLanguageFilter: schoolLang => schoolLang.LanguageId == languageId && schoolLang.Url == url).First();

                return (school: entity.School, schoolLanguage: entity.SchoolLanguage);
            });
        }

        private record SchoolAndLanguageTuple
        {
            public School School { get; set; }
            public Dictionary<int, SchoolLanguages> SchoolLanguage { get; set; }

            public void Deconstruct(out School school, out Dictionary<int, SchoolLanguages> schoolLanguages)
            {
                school = School;
                schoolLanguages = SchoolLanguage;
            }
        }

        private List<SchoolAndLanguageTuple> SchoolWithLanguages(AppDbContext db, Expression<Func<School, bool>> schoolFilter = null, Expression<Func<SchoolLanguages, bool>> schoolLanguageFilter = null)
        {
            var schoolQuery = db.Schools.AsQueryable();
            var schoolLanguageQuery = db.SchoolLanguages.AsQueryable();

            if (schoolFilter is not null)
                schoolQuery = schoolQuery.Where(schoolFilter);

            if (schoolLanguageFilter is not null)
                schoolLanguageQuery = schoolLanguageQuery.Where(schoolLanguageFilter);

            var query = from c in schoolQuery
                let langs = schoolLanguageQuery.Where(lang => lang.SchoolId == c.Id)
                    select new { c, langs };

            var response = query.AsEnumerable().Select(q => new SchoolAndLanguageTuple
            {
                School = q.c,
                SchoolLanguage = q.langs.ToDictionary(lang => lang.LanguageId, lang => lang)
            }).ToList();

            return response;
        }
    }
}