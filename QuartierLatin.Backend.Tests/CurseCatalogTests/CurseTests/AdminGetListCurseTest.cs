using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto.CurseCatalogDto.Curse;
using QuartierLatin.Backend.Models.Repositories.CurseCatalogRepository.CurseRepository;
using QuartierLatin.Backend.Models.Repositories.CurseCatalogRepository.SchoolRepository;
using QuartierLatin.Backend.Tests.CurseCatalogTests.DataSets;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QuartierLatin.Backend.Tests.CurseCatalogTests.CurseTests
{
    public class AdminGetListCurseTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Get_List_CurseAsync(JObject school, int foundationYear, JObject curse)
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

            var curseEntityResponse = SendAdminRequest<List<CurseListAdminDto>>($"/api/admin/curses", null);

            Assert.Equal(id, curseEntityResponse.FirstOrDefault(curse => curse.Id == curseId).SchoolId);
        }

        public static IEnumerable<object[]> Data()
        {
            var school = SchoolDataSet.GetSchool();
            var curse = CurseDataSet.GetCurse(0);

            return new List<object[]>
            {
                new object[]
                {
                    school,
                    1991,
                    curse
                },
            };
        }
    }
}
