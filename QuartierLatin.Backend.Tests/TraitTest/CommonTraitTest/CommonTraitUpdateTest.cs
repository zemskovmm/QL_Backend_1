using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using QuartierLatin.Backend.Tests.TraitTest.TraitTestsDataSet;
using Xunit;

namespace QuartierLatin.Backend.Tests.TraitTest.CommonTraitTest
{
    public class CommonTraitUpdateTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Update_CommonTraitTypeAsync(JObject traitType, string identifier, JObject commonTrait, JObject updatedCommonTrait)
        {
            traitType["identifier"] = identifier;
            var resp = SendAdminRequest<JObject>("/api/admin/trait-types", traitType);
            var id = int.Parse(resp["id"].ToString());
            var repo = GetService<ICommonTraitTypeRepository>();

            var traitTypeEntity = await repo.GetCommonTraitTypeAsync(id);
            Assert.Equal(identifier, traitTypeEntity.Identifier);

            var response = SendAdminRequest<JObject>($"/api/admin/traits/of-type/{id}", commonTrait);

            var commonTraitId = int.Parse(response["id"].ToString());

            var repoCommonTrait = GetService<ICommonTraitRepository>();

            var commonTraitEntity = await repoCommonTrait.GetCommonTraitAsync(commonTraitId);

            Assert.Equal(commonTrait["order"], commonTraitEntity.Order);

            updatedCommonTrait["order"] = 5;
            updatedCommonTrait["traitTypeId"] = id;
            SendAdminRequest<JObject>($"/api/admin/traits/{commonTraitId}", updatedCommonTrait, HttpMethod.Put);

            commonTraitEntity = await repoCommonTrait.GetCommonTraitAsync(commonTraitId);

            Assert.Equal(updatedCommonTrait["order"], commonTraitEntity.Order);
        }

        public static IEnumerable<object[]> Data()
        {
            var traitType = TraitDataSet.GetTraitType();

            var commonTrait = TraitDataSet.GetCommonTrait();

            var updatedCommonTrait = TraitDataSet.GetCommonTraitWithTraitTypeId();

            return new List<object[]>
            {
                new object[]
                {
                    traitType,
                    "test",
                    commonTrait,
                    updatedCommonTrait
                },
            };
        }
    }
}
