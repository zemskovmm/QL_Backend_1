using LinqToDB;
using LinqToDB.Data;
using QuartierLatin.Backend.Models.CourseCatalogModels.CoursesModels;
using QuartierLatin.Backend.Models.Repositories.CourseCatalogRepository.CourseRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartierLatin.Backend.Database.Repositories.CourseCatalogRepository.CourseRepository
{
    public class SqlcourseCatalogRepository : ICourseCatalogRepository
    {
        private readonly AppDbContextManager _db;

        public SqlcourseCatalogRepository(AppDbContextManager db)
        {
            _db = db;
        }

        public async Task<List<(Course course, Dictionary<int, CourseLanguage> courseLanguage)>> GetCourseListAsync()
        {
            return await _db.ExecAsync(async db =>
            {
                var courses = await db.Courses.Select(course => GetCourseByIdAsync(course.Id)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult()).ToListAsync();

                return courses;
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
                var course = await db.Courses.FirstOrDefaultAsync(course => course.Id == id);

                var courseLanguage = await db.CourseLanguages.Where(courseLang => courseLang.CourseId == course.Id)
                    .ToDictionaryAsync(courseLang => courseLang.LanguageId, courseLang => courseLang);

                return (course: course, courseLanguage: courseLanguage);
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
                var courseId =
                    db.CourseLanguages.FirstOrDefault(course =>
                        course.Url == url && course.LanguageId == languageId).CourseId;

                var course = await db.Courses.FirstOrDefaultAsync(course => course.Id == courseId);

                var courseLanguage = await db.CourseLanguages.Where(courseLang => courseLang.CourseId == course.Id)
                    .ToDictionaryAsync(courseLang => courseLang.LanguageId, courseLang => courseLang);

                return (course: course, courseLanguage: courseLanguage);
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
    }
}
