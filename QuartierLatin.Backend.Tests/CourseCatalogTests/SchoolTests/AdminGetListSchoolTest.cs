using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto.courseCatalogDto.School;
using QuartierLatin.Backend.Models.Repositories.courseCatalogRepository.SchoolRepository;
using QuartierLatin.Backend.Tests.CourseCatalogTests.DataSets;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QuartierLatin.Backend.Tests.courseCatalogTests.SchoolTests
{
    public class AdminGetListSchoolTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Get_List_SchoolAsync(JObject school, int foundationYear)
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

            var responseSchool = SendAdminRequest<JObject>($"/api/admin/schools/{id}", null);

            Assert.Equal(foundationYear, responseSchool["foundationYear"]);

            var schoolList = SendAdminRequest<List<SchoolListAdminDto>>($"/api/admin/schools", null);

            Assert.Equal(foundationYear, schoolList.FirstOrDefault(school => school.Id == id).FoundationYear);
        }

        public static IEnumerable<object[]> Data()
        {
            var school = SchoolDataSet.GetSchool();

            return new List<object[]>
            {
                new object[]
                {
                    school,
                    1991
                },
            };
        }
    }
}
