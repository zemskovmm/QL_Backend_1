using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Models.Repositories.CurseCatalogRepository.CurseRepository;
using QuartierLatin.Backend.Models.Repositories.CurseCatalogRepository.SchoolRepository;
using QuartierLatin.Backend.Tests.CurseCatalogTests.DataSets;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using QuartierLatin.Backend.Tests.TraitTest.TraitTestsDataSet;
using Xunit;

namespace QuartierLatin.Backend.Tests.CurseCatalogTests.CurseTests.TraitsTests
{
    public class CommonTraitsToCourseCreateAndGetAndDeleteTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Create_And_Get_And_Delete_CommonTraitsToCourseAsync(JObject school, int foundationYear, JObject course,
            JObject traitType, string identifier, JObject commonTrait)
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

            var curseResp = SendAdminRequest<JObject>("/api/admin/curses", course);
            var curseId = int.Parse(curseResp["id"].ToString());
            var curseRepo = GetService<ICurseCatalogRepository>();
            var curseWithLanguageEntity = await curseRepo.GetCurseByIdAsync(curseId);

            Assert.Equal(id, curseWithLanguageEntity.curse.SchoolId);

            foreach (var curseLanguage in curseWithLanguageEntity.curseLanguage)
            {
                Assert.Equal(curseLanguage.Value.CurseId, curseId);
            }

            traitType["identifier"] = identifier;
            var respTrait = SendAdminRequest<JObject>("/api/admin/trait-types", traitType);
            var idTrait = int.Parse(respTrait["id"].ToString());
            var repoTrait = GetService<ICommonTraitTypeRepository>();

            var traitTypeEntity = await repoTrait.GetCommonTraitTypeAsync(id);
            Assert.Equal(identifier, traitTypeEntity.Identifier);

            var response = SendAdminRequest<JObject>($"/api/admin/traits/of-type/{idTrait}", commonTrait);

            var commonTraitId = int.Parse(response["id"].ToString());

            var repoCommonTrait = GetService<ICommonTraitRepository>();

            var commonTraitEntity = await repoCommonTrait.GetCommonTraitAsync(commonTraitId);

            Assert.Equal(commonTrait["order"], commonTraitEntity.Order);

            SendAdminRequest<JObject>($"/api/admin/entity-traits-curse/{id}/{commonTraitId}", null, HttpMethod.Post);

            var commonTraitCourseList = SendAdminRequest<List<int>>($"/api/admin/entity-traits-curse/{id}", null);

            Assert.Contains(commonTraitId, commonTraitCourseList);

            SendAdminRequest<JObject>($"/api/admin/entity-traits-curse/{id}/{commonTraitId}", null, HttpMethod.Delete);

            commonTraitCourseList = SendAdminRequest<List<int>>($"/api/admin/entity-traits-curse/{id}", null);

            Assert.DoesNotContain(commonTraitId, commonTraitCourseList);
        }

        public static IEnumerable<object[]> Data()
        {
            var school = SchoolDataSet.GetSchool();
            var course = CurseDataSet.GetCurse(0);
            var traitType = TraitDataSet.GetTraitType();
            var commonTrait = TraitDataSet.GetCommonTrait();

            return new List<object[]>
            {
                new object[]
                {
                    school,
                    1991,
                    course,
                    traitType,
                    "test",
                    commonTrait
                },
            };
        }
    }
}
