using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto.TraitTypeDto;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Tests.Infrastructure;
using Xunit;

namespace QuartierLatin.Backend.Tests.TraitTest.TraitTypeTest
{
    public class TraitTypeGetListTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Get_TraitType_ListAsync(JObject traitType, string identifier)
        {
            traitType["identifier"] = identifier;
            var resp = SendAdminRequest<JObject>("/api/admin/trait-types", traitType);
            var id = int.Parse(resp["id"].ToString());
            var repo = GetService<ICommonTraitTypeRepository>();

            var traitTypeEntity = await repo.GetCommonTraitTypeAsync(id);
            Assert.Equal(identifier, traitTypeEntity.Identifier);

            var traitTypeListResponse = SendAdminRequest<List<TraitTypeListDto>>("/api/admin/trait-types", null);

            Assert.Equal(identifier, traitTypeListResponse.FirstOrDefault(trait => trait.Id == id)?.Identifier);
        }

        public static IEnumerable<object[]> Data()
        {
            var traitType = JObject.FromObject(new
            {
                identifier = "test",
                names = new
                {
                    blocks = new object[]
                    {
                        new
                        {
                            type = "text",
                            data = "Lorem ipsum dolor sit amet"
                        },
                        new
                        {
                            type = "standard-service-block",
                            data = new
                            {
                                generalInformation = "Lorem ipsum",
                                advantages = new[] {"Lorem", "ipsum"},
                                requirements = new[] {"Lorem", "ipsum"},
                                prices = new[] {"Lorem", "ipsum"}
                            }
                        }
                    },
                }
            });

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
