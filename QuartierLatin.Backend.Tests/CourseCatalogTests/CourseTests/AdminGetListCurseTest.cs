using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CourseCatalogRepository.CourseRepository;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CourseCatalogRepository.SchoolRepository;
using QuartierLatin.Backend.Dto.CourseCatalogDto.Course;
using QuartierLatin.Backend.Tests.CourseCatalogTests.DataSets;
using QuartierLatin.Backend.Tests.Infrastructure;
using Xunit;

namespace QuartierLatin.Backend.Tests.CourseCatalogTests.CourseTests
{
    public class AdminGetListcourseTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Get_List_courseAsync(JObject school, int foundationYear, JObject course)
        {
            school["foundationYear"] = foundationYear;
            var resp = SendAdminRequest<JObject>("/api/admin/schools", school);
            var id = int.Parse(resp["id"].ToString());
            var repo = GetService<ISchoolCatalogRepository>();

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

            var courseEntityResponse = SendAdminRequest<List<CourseListAdminDto>>($"/api/admin/courses", null);

            Assert.Equal(id, courseEntityResponse.FirstOrDefault(course => course.Id == courseId).SchoolId);
        }

        public static IEnumerable<object[]> Data()
        {
            var school = SchoolDataSet.GetSchool();
            var course = CourseDataSet.GetCourse(0);

            return new List<object[]>
            {
                new object[]
                {
                    school,
                    1991,
                    course
                },
            };
        }
    }
}
