using LinqToDB;
using LinqToDB.Data;
using QuartierLatin.Backend.Models.CourseCatalogModels.CoursesModels;
using QuartierLatin.Backend.Models.Repositories.CourseCatalogRepository.CourseRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Database.Repositories.CourseCatalogRepository.CourseRepository
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

        public async Task<int> CreateCourseAsync(int courseId)
        {
            return await _db.ExecAsync(db => db.InsertWithInt32IdentityAsync(new Course
            {
                SchoolId = courseId,
            }));
        }

        public async Task CreateCourseLanguageListAsync(List<CourseLanguage> courseLanguage)
        {
            await _db.ExecAsync(db => db.BulkCopyAsync(courseLanguage));
        }

        public async Task<(Course course, Dictionary<int, CourseLanguage> courseLanguage)> GetCourseByIdAsync(int id)
        {
            return await _db.ExecAsync(async db =>
            {
                var entity = await CourseWithLanguages(db).FirstOrDefaultAsync(course => course.Course.Id == id);

                return (course: entity.Course, courseLanguage: entity.CourseLanguage);
            });
        }

        public async Task UpdateCourseByIdAsync(int id, int schoolId)
        {
            await _db.ExecAsync(db => db.UpdateAsync(new Course
            {
                Id = id,
                SchoolId = schoolId
            }));
        }

        public async Task CreateOrUpdateCourseLanguageByIdAsync(int id, string htmlDescription, int languageId, string name, string url)
        {
            await _db.ExecAsync(db => db.InsertOrReplaceAsync(new CourseLanguage
            {
                CourseId = id,
                LanguageId = languageId,
                Description = htmlDescription,
                Name = name,
                Url = url
            }));
        }

        public async Task<(Course course, Dictionary<int, CourseLanguage> courseLanguage)> GetCourseByUrlWithLanguageAsync(int languageId, string url)
        {
            return await _db.ExecAsync(async db =>
            {
                var entity = await CourseWithLanguages(db).FirstOrDefaultAsync(course =>
                    course.CourseLanguage.Any(courseLang =>
                        courseLang.Key == languageId && courseLang.Value.Url == url));

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

        private IQueryable<CourseAndLanguageTuple> CourseWithLanguages(AppDbContext db)
        {

            var response = db.Courses.Select(course => new CourseAndLanguageTuple
            {
                Course = course,
                CourseLanguage = db.CourseLanguages.Where(courseLang => courseLang.CourseId == course.Id)
                    .ToDictionary(courseLanguage => courseLanguage.LanguageId, courseLanguage => courseLanguage)
            });

            return response;
        }
    }
}
