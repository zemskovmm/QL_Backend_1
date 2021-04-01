using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using QuartierLatin.Backend.Models.Enums;
using Xunit;

namespace QuartierLatin.Backend.Tests.TraitTest.CommonTraitTest.CommonTraitTypesForEntityTest
{
    public class CommonTraitTypesForEntityCreateAndGetAndDeleteTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Create_And_Get_And_Delete_CommonTraitTypesForEntityAsync(JObject traitType, string identifier, JObject commonTrait)
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

            SendAdminRequest<JObject>($"/api/admin/entity-trait-types/{EntityType.University}/{commonTraitId}", null, HttpMethod.Post);

            var commonTraitTypesForEntityList = SendAdminRequest<List<int>>($"/api/admin/entity-trait-types/{EntityType.University}", null);

            Assert.Contains(commonTraitId, commonTraitTypesForEntityList);

            SendAdminRequest<JObject>($"/api/admin/entity-trait-types/{EntityType.University}/{commonTraitId}", null, HttpMethod.Delete);

            commonTraitTypesForEntityList = SendAdminRequest<List<int>>($"/api/admin/entity-trait-types/{EntityType.University}", null);

            Assert.DoesNotContain(commonTraitId, commonTraitTypesForEntityList);
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

            var commonTrait = JObject.FromObject(new
            {
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
                },
                order = 1
            });

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
