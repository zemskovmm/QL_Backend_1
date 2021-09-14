using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LinqToDB.Common;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CourseCatalogRepository.CourseRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CourseCatalogRepository.SchoolRepository;
using QuartierLatin.Backend.Tests.CourseCatalogTests.DataSets;
using QuartierLatin.Backend.Tests.Infrastructure;
using Xunit;

namespace QuartierLatin.Backend.Tests.CourseCatalogTests.CourseTests
{
    public class AdminUpdateAndAddLanguageCourseTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Configuration.Data))]
        public async Task Admin_Should_Be_Able_To_Update_courseAsync(JObject school, int foundationYear, JObject course, JObject newcourse)
        {
            school["foundationYear"] = foundationYear;
            var resp = SendAdminRequest<JObject>("/api/admin/schools", school);
            var id = int.Parse(resp["id"].ToString());
            var repo = GetService<ISchoolCatalogRepository>();
            var languageRepo = GetService<ILanguageRepository>();

            var schoolWithLanguageEntity = await repo.GetSchoolByIdAsync(id);

            Assert.Equal(foundationYear, schoolWithLanguageEntity.school.FoundationYear);

            foreach (var schoolLanguage in schoolWithLanguageEntity.schoolLanguage)
            {
                Assert.Equal(schoolLanguage.Value.SchoolId, id);
            }

            course["schoolId"] = id;

            var courseResp = SendAdminRequest<JObject>("/api/admin/courses", course);
            var courseId = int.Parse(courseResp["id"].ToString());
            var courseRepo = GetService<ICourseCatalogRepository>();
            var courseWithLanguageEntity = await courseRepo.GetCourseByIdAsync(courseId);

            Assert.Equal(id, courseWithLanguageEntity.course.SchoolId);

            foreach (var courseLanguage in courseWithLanguageEntity.courseLanguage)
            {
                Assert.Equal(courseLanguage.Value.CourseId, courseId);
            }

            newcourse["schoolId"] = id;

            SendAdminRequest<JObject>($"/api/admin/courses/{courseId}", newcourse, HttpMethod.Put);

            var updatedcourse = await courseRepo.GetCourseByIdAsync(courseId);
            var franceId = await languageRepo.GetLanguageIdByShortNameAsync("fr");

            Assert.Equal(newcourse["languages"]["fr"]["name"], updatedcourse.courseLanguage[franceId].Name);
        }

        public static IEnumerable<object[]> Data()
        {
            var school = SchoolDataSet.GetSchool();
            var course = CourseDataSet.GetCourse(0);
            var newcourse = CourseDataSet.GetCourseWithAdditionalLanguage(0);

            return new List<object[]>
            {
                new object[]
                {
                    school,
                    1991,
                    course,
                    newcourse
                },
            };
        }
    }
}
