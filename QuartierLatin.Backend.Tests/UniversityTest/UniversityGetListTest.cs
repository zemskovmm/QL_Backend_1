using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto.UniversityDto.GetUniversityListDto;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Tests.UniversityTest.UniversityTestsDataSet;
using Xunit;

namespace QuartierLatin.Backend.Tests.UniversityTest
{
    public class UniversityGetListTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Get_University_ListAsync(JObject university, string expectedTitle)
        {
            university["website"] = expectedTitle;
            var resp = SendAdminRequest<JObject>("/api/admin/universities", university);
            var id = int.Parse(resp["id"].ToString());
            var repo = GetService<IUniversityRepository>();
            var languageRepo = GetService<ILanguageRepository>();

            var universityEntity = await repo.GetUniversityByIdAsync(id);
            Assert.Equal(expectedTitle, universityEntity.Website);

            var universityLanguageResponse = SendAdminRequest<List<UniversityListDto>>("/api/admin/universities", null);

            var universityLanguageEntity = await repo.GetUniversityLanguageByUniversityIdAsync(id);

            foreach (var universityLanguage in universityLanguageEntity)
            {
                var lang = await languageRepo.GetLanguageShortNameAsync(universityLanguage.Key);

                var univesityFromResponse = universityLanguageResponse
                    .FirstOrDefault(univesityLanguageResp =>
                        univesityLanguageResp.Id == universityLanguage.Value.UniversityId);

                Assert.Equal(universityLanguage.Value.Description, univesityFromResponse.Languages[lang].HtmlDescription);
                Assert.Equal(universityLanguage.Value.Url, univesityFromResponse.Languages[lang].Url);
            }
        }

        public static IEnumerable<object[]> Data()
        {
            var university = UniversityDataSet.GetUniversity();

            return new List<object[]>
            {
                new object[]
                {
                    university,
                    "http://test.ru"
                },
            };
        }
    }
}
