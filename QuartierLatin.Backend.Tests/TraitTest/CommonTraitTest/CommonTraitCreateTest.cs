using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Tests.TraitTest.TraitTestsDataSet;
using Xunit;

namespace QuartierLatin.Backend.Tests.TraitTest.CommonTraitTest
{
    public class CommonTraitCreateTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Create_CommonTraitTypeAsync(JObject traitType, string identifier, JObject commonTrait)
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
        }

        public static IEnumerable<object[]> Data()
        {
            var traitType = TraitDataSet.GetTraitType();

            var commonTrait = TraitDataSet.GetCommonTrait();

            return new List<object[]>
            {
                new object[]
                {
                    traitType,
                    "test",
                    commonTrait
                },
            };
        }
    }
}
