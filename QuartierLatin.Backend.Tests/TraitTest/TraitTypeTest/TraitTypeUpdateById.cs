using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto.TraitTypeDto;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Tests.Infrastructure;
using QuartierLatin.Backend.Tests.TraitTest.TraitTestsDataSet;
using Xunit;

namespace QuartierLatin.Backend.Tests.TraitTest.TraitTypeTest
{
    public class TraitTypeUpdateById : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Update_TraitTypeAsync(JObject traitType, string identifier)
        {
            var resp = SendAdminRequest<JObject>("/api/admin/trait-types", traitType);
            var id = int.Parse(resp["id"].ToString());
            var repo = GetService<ICommonTraitTypeRepository>();

            var traitTypeEntity = await repo.GetCommonTraitTypeAsync(id);
            Assert.Equal(traitType["identifier"], traitTypeEntity.Identifier);

            traitType["identifier"] = identifier;

            SendAdminRequest<TraitTypeListDto>($"/api/admin/trait-types/{id}", traitType, HttpMethod.Put);

            traitTypeEntity = await repo.GetCommonTraitTypeAsync(id);

            Assert.Equal(identifier, traitTypeEntity.Identifier);
        }

        public static IEnumerable<object[]> Data()
        {
            var traitType = TraitDataSet.GetTraitType();

            return new List<object[]>
            {
                new object[]
                {
                    traitType,
                    "test"
                },
            };
        }
    }
}
