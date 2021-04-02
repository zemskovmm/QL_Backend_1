using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using QuartierLatin.Backend.Tests.TraitTest.TraitTestsDataSet;
using QuartierLatin.Backend.Tests.UniversityTest.UniversityTestsDataSet;
using Xunit;

namespace QuartierLatin.Backend.Tests.TraitTest.CommonTraitTest.CommonTraitsToUniversityTest
{
    public class CommonTraitsToUniversityCreateAndGetAndDeleteTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Create_And_Get_And_Delete_CommonTraitsToUniversityAsync(JObject university, string expectedTitle, JObject traitType, string identifier, JObject commonTrait)
        {
            university["website"] = expectedTitle;
            var resp = SendAdminRequest<JObject>("/api/admin/universities", university);
            var id = int.Parse(resp["id"].ToString());
            var repo = GetService<IUniversityRepository>();
            var languageRepo = GetService<ILanguageRepository>();

            var universityEntity = await repo.GetUniversityByIdAsync(id);
            Assert.Equal(expectedTitle, universityEntity.Website);

            var universityLanguageEntity = await repo.GetUniversityLanguageByUniversityIdAsync(id);

            foreach (var universityLanguage in universityLanguageEntity)
            {
                var lang = await languageRepo.GetLanguageShortNameAsync(universityLanguage.Key);

                Assert.Equal(universityLanguage.Value.UniversityId, id);
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

            SendAdminRequest<JObject>($"/api/admin/entity-traits-university/{id}/{commonTraitId}", null, HttpMethod.Post);

            var commonTraitUniversityList = SendAdminRequest<List<int>>($"/api/admin/entity-traits-university/{id}", null);

            Assert.Contains(commonTraitId, commonTraitUniversityList);

            SendAdminRequest<JObject>($"/api/admin/entity-traits-university/{id}/{commonTraitId}", null, HttpMethod.Delete);

            commonTraitUniversityList = SendAdminRequest<List<int>>($"/api/admin/entity-traits-university/{id}", null);

            Assert.DoesNotContain(commonTraitId, commonTraitUniversityList);
        }

        public static IEnumerable<object[]> Data()
        {
            var university = UniversityDataSet.GetUniversity();

            var traitType = TraitDataSet.GetTraitType();

            var commonTrait = TraitDataSet.GetCommonTrait();

            return new List<object[]>
            {
                new object[]
                {
                    university,
                    "http://test.ru",
                    traitType,
                    "test",
                    commonTrait
                },
            };
        }
    }
}
