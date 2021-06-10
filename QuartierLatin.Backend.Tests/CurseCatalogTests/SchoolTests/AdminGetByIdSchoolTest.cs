using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Repositories.CurseCatalogRepository.SchoolRepository;
using QuartierLatin.Backend.Tests.CurseCatalogTests.DataSets;
using QuartierLatin.Backend.Tests.Infrastructure;
using Xunit;

namespace QuartierLatin.Backend.Tests.CurseCatalogTests.SchoolTests
{
    public class AdminGetByIdSchoolTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Get_School_By_IdAsync(JObject school, int foundationYear)
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
