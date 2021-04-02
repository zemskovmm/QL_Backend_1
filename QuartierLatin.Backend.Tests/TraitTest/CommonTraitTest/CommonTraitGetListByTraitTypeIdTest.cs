using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Dto.CommonTraitDto;
using QuartierLatin.Backend.Tests.TraitTest.TraitTestsDataSet;
using Xunit;

namespace QuartierLatin.Backend.Tests.TraitTest.CommonTraitTest
{
    public class CommonTraitGetListByTraitTypeIdTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Get_CommonTraitType_List_By_TraitType_IdAsync(JObject traitType, string identifier, JObject commonTrait)
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

            var responseCommonTraitList = SendAdminRequest<List<CommonTraitListDto>>($"/api/admin/traits/of-type/{id}", null);

            Assert.Equal(commonTraitEntity.CommonTraitTypeId, responseCommonTraitList.FirstOrDefault(trait => trait.Id == commonTraitId).CommonTraitTypeId);
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
