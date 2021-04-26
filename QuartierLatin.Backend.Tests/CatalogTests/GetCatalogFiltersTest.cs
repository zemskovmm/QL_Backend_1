using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto.CatalogDto;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Tests.Infrastructure;
using QuartierLatin.Backend.Tests.TraitTest.TraitTestsDataSet;
using QuartierLatin.Backend.Tests.UniversityTest.UniversityTestsDataSet;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace QuartierLatin.Backend.Tests.CatalogTests
{
    public class GetCatalogFiltersTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Anon_User_Should_Be_Able_To_Get_Catalog_Filter_ListAsync(JObject university, string expectedTitle, JObject commonTrait,
            JObject cityTraitType, JObject degreeTraitType)
        {
            university["website"] = expectedTitle;
            var resp = SendAdminRequest<JObject>("/api/admin/universities", university);
            var id = int.Parse(resp["id"].ToString());
            var repo = GetService<IUniversityRepository>();

            var universityEntity = await repo.GetUniversityByIdAsync(id);
            Assert.Equal(expectedTitle, universityEntity.Website);

            var universityLanguageEntity = await repo.GetUniversityLanguageByUniversityIdAsync(id);

            foreach (var universityLanguage in universityLanguageEntity)
            {
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

            SendAdminRequest<JObject>($"/api/admin/entity-trait-types/{EntityType.University}/{commonTraitCityId}", null, HttpMethod.Post);
            SendAdminRequest<JObject>($"/api/admin/entity-trait-types/{EntityType.University}/{commonTraitDegreeId}", null, HttpMethod.Post);

            var commonTraitUniversityIdList = SendAdminRequest<List<int>>($"/api/admin/entity-traits-university/{id}", null);

            Assert.Contains(commonTraitCityId, commonTraitUniversityIdList);
            Assert.Contains(commonTraitDegreeId, commonTraitUniversityIdList);

            var catalogFilterResponse = SendAnonRequest<CatalogFilterResponseDto>($"/api/catalog/filters/university/ru", null);
            
            Assert.NotNull(catalogFilterResponse.Filters
                .FirstOrDefault(filter => filter.Identifier == cityTraitType["identifier"].ToString()));

            Assert.NotNull(catalogFilterResponse.Filters
                .FirstOrDefault(filter => filter.Identifier == degreeTraitType["identifier"].ToString()));

            Assert.Equal(commonTrait["names"]["ru"],catalogFilterResponse.Filters
                .FirstOrDefault(filter => filter.Identifier == cityTraitType["identifier"].ToString())
                    .Options.FirstOrDefault(option => option.Id == commonTraitCityId).Name);

            Assert.Equal(commonTrait["names"]["ru"], catalogFilterResponse.Filters
                .FirstOrDefault(filter => filter.Identifier == degreeTraitType["identifier"].ToString())
                    .Options.FirstOrDefault(option => option.Id == commonTraitDegreeId).Name);
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
