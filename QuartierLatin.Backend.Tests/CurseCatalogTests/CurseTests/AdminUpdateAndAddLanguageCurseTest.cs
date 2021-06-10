using LinqToDB.Common;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Models.Repositories.CurseCatalogRepository.CurseRepository;
using QuartierLatin.Backend.Models.Repositories.CurseCatalogRepository.SchoolRepository;
using QuartierLatin.Backend.Tests.CurseCatalogTests.DataSets;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace QuartierLatin.Backend.Tests.CurseCatalogTests.CurseTests
{
    public class AdminUpdateAndAddLanguageCurseTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Configuration.Data))]
        public async Task Admin_Should_Be_Able_To_Update_CurseAsync(JObject school, int foundationYear, JObject curse, JObject newCurse)
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

            curse["schoolId"] = id;

            var curseResp = SendAdminRequest<JObject>("/api/admin/curses", curse);
            var curseId = int.Parse(curseResp["id"].ToString());
            var curseRepo = GetService<ICurseCatalogRepository>();
            var curseWithLanguageEntity = await curseRepo.GetCurseByIdAsync(curseId);

            Assert.Equal(id, curseWithLanguageEntity.curse.SchoolId);

            foreach (var curseLanguage in curseWithLanguageEntity.curseLanguage)
            {
                Assert.Equal(curseLanguage.Value.CurseId, curseId);
            }

            newCurse["schoolId"] = id;

            SendAdminRequest<JObject>($"/api/admin/curses/{curseId}", newCurse, HttpMethod.Put);

            var updatedCurse = await curseRepo.GetCurseByIdAsync(curseId);
            var franceId = await languageRepo.GetLanguageIdByShortNameAsync("fr");

            Assert.Equal(newCurse["languages"]["fr"]["name"], updatedCurse.curseLanguage[franceId].Name);
        }

        public static IEnumerable<object[]> Data()
        {
            var school = SchoolDataSet.GetSchool();
            var curse = CurseDataSet.GetCurse(0);
            var newCurse = CurseDataSet.GetCurseWithAdditionalLanguage(0);

            return new List<object[]>
            {
                new object[]
                {
                    school,
                    1991,
                    curse,
                    newCurse
                },
            };
        }
    }
}
