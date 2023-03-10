using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CourseCatalogRepository.SchoolRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CourseCatalogModels.SchoolModels;
using QuartierLatin.Backend.Utils;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Repositories.CourseCatalogRepository.SchoolRepository
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
                var entity = SchoolWithLanguages(db).ToList();

                var response = entity.Select(resp => (resp.School, resp.SchoolLanguage)).ToList();

                return response;
            });
        }

        public async Task<int> CreateSchoolAsync(int? foundationYear, int? imageId, List<SchoolLanguages> schoolLanguage)
        {
            return await _db.ExecAsync(db => db.InTransaction(async () =>
            {
                var schoolId = await db.InsertWithInt32IdentityAsync(new School
                {
                    FoundationYear = foundationYear,
                    ImageId = imageId
                });

                schoolLanguage.ForEach(schoolLang => schoolLang.SchoolId = schoolId);
                await db.BulkCopyAsync(schoolLanguage);

                return schoolId;
            }));
        }


        public async Task<(School school, Dictionary<int, SchoolLanguages> schoolLanguage)> GetSchoolByIdAsync(int id)
        {
            return await _db.ExecAsync(async db =>
            {
                var entity = SchoolWithLanguages(db, school => school.Id == id).First();

                return (school: entity.School, schoolLanguage: entity.SchoolLanguage);
            });
        }

        public async Task UpdateSchoolByIdAsync(int id, int? foundationYear, int? imageId)
        {
            await _db.ExecAsync(db => db.UpdateAsync(new School
            {
                Id = id,
                FoundationYear = foundationYear,
                ImageId = imageId
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

        public async Task<Dictionary<int, (int? schoolImageId, string schoolName, string schoolUrl)>> GetSchoolImageIdAndNameAndUrlByIdsAsync(IEnumerable<int> schoolIds, string lang)
        {
            return await _db.ExecAsync(async db =>
            {
                var langId = db.Languages.FirstOrDefault(language => language.LanguageShortName == lang).Id;

                var schoolWithLanguages = from school in db.Schools.Where(school => schoolIds.Contains(school.Id))
                    join lang in db.SchoolLanguages.Where(l => l.LanguageId == langId)
                        on school.Id equals lang.SchoolId
                    select new
                    {
                        school.Id,
                        school.ImageId,
                        lang.Name,
                        lang.Url
                    };

                return await schoolWithLanguages.ToDictionaryAsync(school => school.Id,
                    school => (school.ImageId, school.Name, school.Url));
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
            {
                schoolLanguageQuery = schoolLanguageQuery.Where(schoolLanguageFilter);

                var schoolIds = schoolLanguageQuery.Select(lang => lang.SchoolId).Distinct();

                schoolQuery = schoolQuery.Where(school => schoolIds.Contains(school.Id));
            }

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