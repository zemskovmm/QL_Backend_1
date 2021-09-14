using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CourseCatalogRepository.SchoolRepository;
using QuartierLatin.Backend.Dto;
using QuartierLatin.Backend.Dto.CourseCatalogDto.School.ModuleDto;
using QuartierLatin.Backend.Tests.CourseCatalogTests.DataSets;
using QuartierLatin.Backend.Tests.Infrastructure;
using QuartierLatin.Backend.Tests.TraitTest.TraitTestsDataSet;
using Xunit;

namespace QuartierLatin.Backend.Tests.CourseCatalogTests.SchoolTests.RoutingTest
{
    public class SchoolRouteTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Anon_User_Should_Be_Able_To_Get_School_By_UrlAsync(JObject school, int foundationYear, JObject commonTrait,
            JObject cityTraitType, JObject degreeTraitType)
        {
            school["foundationYear"] = foundationYear;
            var resp = SendAdminRequest<JObject>("/api/admin/schools", school);
            var id = int.Parse(resp["id"].ToString());
            var repo = GetService<ISchoolCatalogRepository>();

            var schoolWithLanguageEntity = await repo.GetSchoolByIdAsync(id);

            Assert.Equal(foundationYear, schoolWithLanguageEntity.school.FoundationYear);

            foreach (var schoolLanguage in schoolWithLanguageEntity.schoolLanguage)
            {
                Assert.Equal(schoolLanguage.Value.SchoolId, id);
            }

            var respTraitCity = SendAdminRequest<JObject>("/api/admin/trait-types", cityTraitType);
            var respTraitDegree = SendAdminRequest<JObject>("/api/admin/trait-types", degreeTraitType);

            var traitCityId = int.Parse(respTraitCity["id"].ToString());
            var traitDegreeId = int.Parse(respTraitDegree["id"].ToString());

            var responseCommonTraitCity = SendAdminRequest<JObject>($"/api/admin/traits/of-type/{traitCityId}", commonTrait);
            var responseCommonTraitDegree = SendAdminRequest<JObject>($"/api/admin/traits/of-type/{traitDegreeId}", commonTrait);

            var commonTraitCityId = int.Parse(responseCommonTraitCity["id"].ToString());
            var commonTraitDegreeId = int.Parse(responseCommonTraitDegree["id"].ToString());

            SendAdminRequest<JObject>($"/api/admin/entity-traits-school/{id}/{commonTraitCityId}", null, HttpMethod.Post);
            SendAdminRequest<JObject>($"/api/admin/entity-traits-school/{id}/{commonTraitDegreeId}", null, HttpMethod.Post);

            var commonTraitUniversityIdList = SendAdminRequest<List<int>>($"/api/admin/entity-traits-school/{id}", null);

            Assert.Contains(commonTraitCityId, commonTraitUniversityIdList);
            Assert.Contains(commonTraitDegreeId, commonTraitUniversityIdList);

            var routeResponse =
                SendAnonRequest<RouteDto<SchoolModuleDto>>(
                    $"/api/route/ru/school/{school["languages"]["ru"]["url"]}", null);
            Assert.Equal(school["languages"]["ru"]["name"].ToString(), routeResponse.Module.Title);
            Assert.Equal(school["languages"]["ru"]["htmlDescription"].ToString(), routeResponse.Module.DescriptionHtml);
        }

        public static IEnumerable<object[]> Data()
        {
            var school = SchoolDataSet.GetSchool();
            var commonTrait = TraitDataSet.GetCommonTrait();
            var cityTraitType = TraitDataSet.GetTraitType("city");
            var degreeTraitType = TraitDataSet.GetTraitType("degree");

            return new List<object[]>
            {
                new object[]
                {
                    school,
                    1991,
                    commonTrait,
                    cityTraitType,
                    degreeTraitType
                },
            };
        }
    }
}
