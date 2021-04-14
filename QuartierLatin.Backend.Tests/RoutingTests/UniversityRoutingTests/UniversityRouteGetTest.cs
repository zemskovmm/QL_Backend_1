using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto;
using QuartierLatin.Backend.Dto.UniversityDto;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Tests.Infrastructure;
using QuartierLatin.Backend.Tests.TraitTest.TraitTestsDataSet;
using QuartierLatin.Backend.Tests.UniversityTest.UniversityTestsDataSet;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace QuartierLatin.Backend.Tests.RoutingTests.UniversityRoutingTests
{
    public class UniversityRouteGetTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Anon_User_Should_Be_Able_To_Get_University_By_UrlAsync(JObject university, string expectedTitle, JObject commonTrait,
            JObject cityTraitType, JObject degreeTraitType)
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

            var respTraitCity = SendAdminRequest<JObject>("/api/admin/trait-types", cityTraitType);
            var respTraitDegree = SendAdminRequest<JObject>("/api/admin/trait-types", degreeTraitType);

            var traitCityId = int.Parse(respTraitCity["id"].ToString());
            var traitDegreeId = int.Parse(respTraitDegree["id"].ToString());

            var responseCommonTraitCity = SendAdminRequest<JObject>($"/api/admin/traits/of-type/{traitCityId}", commonTrait);
            var responseCommonTraitDegree = SendAdminRequest<JObject>($"/api/admin/traits/of-type/{traitDegreeId}", commonTrait);

            var commonTraitCityId = int.Parse(responseCommonTraitCity["id"].ToString());
            var commonTraitDegreeId = int.Parse(responseCommonTraitDegree["id"].ToString());

            SendAdminRequest<JObject>($"/api/admin/entity-traits-university/{id}/{commonTraitCityId}", null, HttpMethod.Post);
            SendAdminRequest<JObject>($"/api/admin/entity-traits-university/{id}/{commonTraitDegreeId}", null, HttpMethod.Post);

            var commonTraitUniversityIdList = SendAdminRequest<List<int>>($"/api/admin/entity-traits-university/{id}", null);

            Assert.Contains(commonTraitCityId, commonTraitUniversityIdList);
            Assert.Contains(commonTraitDegreeId, commonTraitUniversityIdList);

            var routeResponse =
                SendAnonRequest<RouteDto<UniversityModuleDto>>(
                    $"/api/route/ru/university/{university["languages"]["ru"]["url"]}", null);
            Assert.Equal(university["languages"]["ru"]["name"].ToString(), routeResponse.Module.Title);
            Assert.Equal(university["languages"]["ru"]["htmlDescription"].ToString(), routeResponse.Module.DescriptionHtml);
        }

        public static IEnumerable<object[]> Data()
        {
            var university = UniversityDataSet.GetUniversity();

            var commonTrait = TraitDataSet.GetCommonTrait();

            var cityTraitType = TraitDataSet.GetTraitType("city");
            var degreeTraitType = TraitDataSet.GetTraitType("degree");

            return new List<object[]>
            {
                new object[]
                {
                    university,
                    "http://test.ru",
                    commonTrait,
                    cityTraitType,
                    degreeTraitType
                },
            };
        }
    }
}
