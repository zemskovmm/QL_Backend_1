using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CourseCatalogRepository.SchoolRepository;
using QuartierLatin.Backend.Tests.CourseCatalogTests.DataSets;
using QuartierLatin.Backend.Tests.Infrastructure;
using Xunit;

namespace QuartierLatin.Backend.Tests.CourseCatalogTests.SchoolTests
{
    public class AdminUpdateAndAddLanguageSchoolTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Update_SchoolAsync(JObject school, int foundationYear, JObject schoolNew)
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

            var responseSchool = SendAdminRequest<JObject>($"/api/admin/schools/{id}", null);

            Assert.Equal(foundationYear, responseSchool["foundationYear"]);

            SendAdminRequest<JObject>($"/api/admin/schools/{id}", schoolNew, HttpMethod.Put);

            var schoolNewEntity = await repo.GetSchoolByIdAsync(id);

            var franceId = await languageRepo.GetLanguageIdByShortNameAsync("fr");

            Assert.Equal(schoolNew["languages"]["fr"]["name"], schoolNewEntity.schoolLanguage[franceId].Name);
        }

        public static IEnumerable<object[]> Data()
        {
            var school = SchoolDataSet.GetSchool();
            var schoolNew = SchoolDataSet.GetSchoolWithAdditionalLanguage();

            return new List<object[]>
            {
                new object[]
                {
                    school,
                    1991,
                    schoolNew
                },
            };
        }
    }
}
