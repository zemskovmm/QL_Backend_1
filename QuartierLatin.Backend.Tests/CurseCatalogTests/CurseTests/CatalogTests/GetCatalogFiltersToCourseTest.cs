using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto.CatalogDto;
using QuartierLatin.Backend.Models.Enums;
using QuartierLatin.Backend.Models.Repositories.CurseCatalogRepository.CurseRepository;
using QuartierLatin.Backend.Models.Repositories.CurseCatalogRepository.SchoolRepository;
using QuartierLatin.Backend.Tests.CurseCatalogTests.DataSets;
using QuartierLatin.Backend.Tests.Infrastructure;
using QuartierLatin.Backend.Tests.TraitTest.TraitTestsDataSet;
using Xunit;

namespace QuartierLatin.Backend.Tests.CurseCatalogTests.CurseTests.CatalogTests
{
    public class GetCatalogFiltersToCourseTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Anon_User_Should_Be_Able_To_Get_Catalog_Filter_To_Course_ListAsync(JObject school,
            int foundationYear, JObject curse, JObject commonTrait,
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

            curse["schoolId"] = id;

            var curseResp = SendAdminRequest<JObject>("/api/admin/curses", curse);
            var curseId = int.Parse(curseResp["id"].ToString());
            var curseRepo = GetService<ICurseCatalogRepository>();
            var curseWithLanguageEntity = await curseRepo.GetCurseByIdAsync(curseId);

            Assert.Equal(id, curseWithLanguageEntity.curse.SchoolId);

            foreach (var curseLanguage in curseWithLanguageEntity.curseLanguage)
            {
                Assert.Equal(curseLanguage.Value.CurseId, curseId);
            }

            var respTraitCity = SendAdminRequest<JObject>("/api/admin/trait-types", cityTraitType);
            var respTraitDegree = SendAdminRequest<JObject>("/api/admin/trait-types", degreeTraitType);

            var traitCityId = int.Parse(respTraitCity["id"].ToString());
            var traitDegreeId = int.Parse(respTraitDegree["id"].ToString());

            var responseCommonTraitCity =
                SendAdminRequest<JObject>($"/api/admin/traits/of-type/{traitCityId}", commonTrait);
            var responseCommonTraitDegree =
                SendAdminRequest<JObject>($"/api/admin/traits/of-type/{traitDegreeId}", commonTrait);

            var commonTraitCityId = int.Parse(responseCommonTraitCity["id"].ToString());
            var commonTraitDegreeId = int.Parse(responseCommonTraitDegree["id"].ToString());

            SendAdminRequest<JObject>($"/api/admin/entity-traits-curse/{curseId}/{commonTraitCityId}", null,
                HttpMethod.Post);
            SendAdminRequest<JObject>($"/api/admin/entity-traits-curse/{curseId}/{commonTraitDegreeId}", null,
                HttpMethod.Post);

            SendAdminRequest<JObject>($"/api/admin/entity-trait-types/{EntityType.School}/{commonTraitCityId}",
                null, HttpMethod.Post);
            SendAdminRequest<JObject>($"/api/admin/entity-trait-types/{EntityType.School}/{commonTraitDegreeId}",
                null, HttpMethod.Post);

            var commonTraitUniversityIdList =
                SendAdminRequest<List<int>>($"/api/admin/entity-traits-curse/{curseId}", null);

            Assert.Contains(commonTraitCityId, commonTraitUniversityIdList);
            Assert.Contains(commonTraitDegreeId, commonTraitUniversityIdList);

            var catalogFilterResponse =
                SendAnonRequest<CatalogFilterResponseDto>($"/api/catalog/curse/filters/ru", null);

            Assert.NotNull(catalogFilterResponse.Filters
                .FirstOrDefault(filter => filter.Identifier == cityTraitType["identifier"].ToString()));

            Assert.NotNull(catalogFilterResponse.Filters
                .FirstOrDefault(filter => filter.Identifier == degreeTraitType["identifier"].ToString()));

            Assert.Equal(commonTrait["names"]["ru"], catalogFilterResponse.Filters
                .FirstOrDefault(filter => filter.Identifier == cityTraitType["identifier"].ToString())
                .Options.FirstOrDefault(option => option.Id == commonTraitCityId).Name);

            Assert.Equal(commonTrait["names"]["ru"], catalogFilterResponse.Filters
                .FirstOrDefault(filter => filter.Identifier == degreeTraitType["identifier"].ToString())
                .Options.FirstOrDefault(option => option.Id == commonTraitDegreeId).Name);
        }

        public static IEnumerable<object[]> Data()
        {
            var school = SchoolDataSet.GetSchool();
            var curse = CurseDataSet.GetCurse(0);
            var commonTrait = TraitDataSet.GetCommonTrait();
            var cityTraitType = TraitDataSet.GetTraitType("city");
            var degreeTraitType = TraitDataSet.GetTraitType("degree");


            return new List<object[]>
            {
                new object[]
                {
                    school,
                    1991,
                    curse,
                    commonTrait,
                    cityTraitType,
                    degreeTraitType
                },
            };

        }
    }
}
