using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CourseCatalogRepository.CourseRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CourseCatalogModels.CoursesModels;
using QuartierLatin.Backend.Utils;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Repositories.CourseCatalogRepository.CourseRepository
{
    public class SqlCourseCatalogRepository : ICourseCatalogRepository
    {
        private readonly AppDbContextManager _db;

        public SqlCourseCatalogRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<List<(Course course, Dictionary<int, CourseLanguage> courseLanguage)>> GetCourseListAsync()
        {
            return await _db.ExecAsync(async db =>
            {
                var entity = CourseWithLanguages(db).ToList();

                var response = entity.Select(resp => (resp.Course, resp.CourseLanguage)).ToList();

                return response;
            });
        }

        public async Task<int> CreateCourseAsync(int schoolId, int? imageId, int price, List<CourseLanguage> courseLanguage)
        {
            return await _db.ExecAsync(db => db.InTransaction(async () =>
            {
                var courseId = await db.InsertWithInt32IdentityAsync(new Course
                {
                    SchoolId = schoolId,
                    ImageId = imageId,
                    Price = price
                });

                courseLanguage.ForEach(courseLang => courseLang.CourseId = courseId);
                await db.BulkCopyAsync(courseLanguage);

                return courseId;
            }));
        }


        public async Task<(Course course, Dictionary<int, CourseLanguage> courseLanguage)> GetCourseByIdAsync(int id)
        {
            return await _db.ExecAsync(async db =>
            {
                var entity = CourseWithLanguages(db, course => course.Id == id).First();

                return (course: entity.Course, courseLanguage: entity.CourseLanguage);
            });
        }

        public async Task UpdateCourseByIdAsync(int id, int schoolId, int? imageId, int price)
        {
            await _db.ExecAsync(db => db.UpdateAsync(new Course
            {
                Id = id,
                SchoolId = schoolId,
                ImageId = imageId,
                Price = price
            }));
        }

        public async Task CreateOrUpdateCourseLanguageByIdAsync(int id, string htmlDescription, int languageId, string name, string url, JObject? metadata)
        {
            await _db.ExecAsync(db => db.InsertOrReplaceAsync(new CourseLanguage
            {
                CourseId = id,
                LanguageId = languageId,
                Description = htmlDescription,
                Name = name,
                Url = url,
                Metadata = metadata?.ToString()
            }));
        }

        public async Task<(Course course, Dictionary<int, CourseLanguage> courseLanguage)> GetCourseByUrlWithLanguageAsync(int languageId, string url)
        {
            return await _db.ExecAsync(async db =>
            {
                var entity = CourseWithLanguages(db,
                    languageFilter: courseLang => courseLang.LanguageId == languageId && courseLang.Url == url).First();

                return (course: entity.Course, courseLanguage: entity.CourseLanguage);
            });
        }

        public async Task<(int totalItems, List<(Course course, CourseLanguage courseLanguage)>)> GetCoursePageByFilter(List<List<int>> commonTraitsIds, int langId, int skip, int take)
        {
            return await _db.ExecAsync(async db =>
            {
                var courses = db.Courses.AsQueryable();

                if (commonTraitsIds.Any())
                {
                    var courseWithTraits = db.CommonTraitToCourses.AsQueryable();
                    var schoolWithTrait = db.CommonTraitToSchools.AsQueryable();

                    foreach (var commonTraitGroup in commonTraitsIds)
                    {
                        if (commonTraitGroup.Count != 0)
                            schoolWithTrait =
                                schoolWithTrait.Where(t => commonTraitGroup.Contains(t.CommonTraitId));
                    }

                    foreach (var commonTraitGroup in commonTraitsIds)
                    {
                        if (commonTraitGroup.Count != 0)
                            courseWithTraits =
                                courseWithTraits.Where(t => commonTraitGroup.Contains(t.CommonTraitId));
                    }

                    courses = courses.Where(course =>
                        courseWithTraits.Select(x => x.CourseId).Contains(course.Id) &&
                        schoolWithTrait.Select(x => x.SchoolId).Contains(course.SchoolId));
                }

                var courseWithLanguages = from course in courses
                                                join lang in db.CourseLanguages.Where(l => l.LanguageId == langId)
                                                    on course.Id equals lang.CourseId
                                                select new
                                                {
                                                    course,
                                                    lang
                                                };

                var totalCount = await courseWithLanguages.CountAsync();

                return (totalCount,
                    (await courseWithLanguages.OrderBy(x => x.course.Id).Skip(skip).Take(take).ToListAsync())
                    .Select(x => (course: x.course, courseLanguage: x.lang)).ToList());
            });
        }

        public async Task<List<string>> GetCourseUrlsListAsync(int schoolId)
        {
            return await _db.ExecAsync(async db =>
            {
                var courseIds = db.Courses.Where(course => course.SchoolId == schoolId).Select(course => course.Id);

                return await db.CourseLanguages.Where(courseLang => courseIds.Contains(courseLang.CourseId))
                    .Select(courseLang => courseLang.Url).ToListAsync();
            });
        }

        public async Task<List<(Course course, Dictionary<int, CourseLanguage> courseLanguage)>> GetCoursesListAsync(int schoolId)
        {
            return await _db.ExecAsync(async db =>
            {
                var entity =  CourseWithLanguages(db, course => course.SchoolId == schoolId);
                var response = entity.Select(resp => (resp.Course, resp.CourseLanguage)).ToList();

                return response;
            });
        }

        private record CourseAndLanguageTuple
        {
            public Course Course { get; set; }
            public Dictionary<int, CourseLanguage> CourseLanguage { get; set; }

            public void Deconstruct(out Course course, out Dictionary<int, CourseLanguage> courseLanguage)
            {
                course = Course;
                courseLanguage = CourseLanguage;
            }
        }

        private List<CourseAndLanguageTuple> CourseWithLanguages(AppDbContext db, Expression <Func<Course, bool>> courseFilter = null, Expression<Func<CourseLanguage, bool>> languageFilter = null)
        {

            var courseQuery = db.Courses.AsQueryable();
            var languageQuery = db.CourseLanguages.AsQueryable();

            if (courseFilter is not null)
                courseQuery = courseQuery.Where(courseFilter);

            if (languageFilter is not null)
                languageQuery = languageQuery.Where(languageFilter);

            var q = from c in courseQuery
                let langs = languageQuery.Where(lang => lang.CourseId == c.Id)
                    select new {c, langs};

            return q.AsEnumerable().Select(x => new CourseAndLanguageTuple
            {
                Course = x.c,
                CourseLanguage = x.langs.ToDictionary(courseLang => courseLang.LanguageId, courseLang => courseLang)
            }).ToList();
        }
    }
}