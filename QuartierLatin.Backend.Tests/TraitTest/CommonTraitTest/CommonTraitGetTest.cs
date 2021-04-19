using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto.CommonTraitDto;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Tests.TraitTest.TraitTestsDataSet;
using Xunit;

namespace QuartierLatin.Backend.Tests.TraitTest.CommonTraitTest
{
    public class CommonTraitGetTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Get_CommonTraitType_By_IdAsync(JObject traitType, string identifier, JObject commonTrait)
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

            var responseCommonTraitList = SendAdminRequest<CommonTraitListDto>($"/api/admin/traits/{commonTraitId}", null);

            Assert.Equal(commonTraitEntity.CommonTraitTypeId, responseCommonTraitList.CommonTraitTypeId);
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
