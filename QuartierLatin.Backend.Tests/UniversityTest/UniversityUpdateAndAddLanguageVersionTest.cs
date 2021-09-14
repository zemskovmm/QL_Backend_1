using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto.UniversityDto.GetUniversityListDto;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Tests.UniversityTest.UniversityTestsDataSet;
using Xunit;

namespace QuartierLatin.Backend.Tests.UniversityTest
{
    public class UniversityUpdateAndAddLanguageVersionTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Get_University_ByIdAsync(JObject university, string expectedTitle, JObject universityUpdated)
        {
            university["website"] = expectedTitle;
            var resp = SendAdminRequest<JObject>("/api/admin/universities", university);
            var id = int.Parse(resp["id"].ToString());
            var repo = GetService<IUniversityRepository>();
            var languageRepo = GetService<ILanguageRepository>();

            var universityEntity = await repo.GetUniversityByIdAsync(id);
            //FIXME: removed
            //Assert.Equal(expectedTitle, universityEntity.Website);

            SendAdminRequest<UniversityListDto>($"/api/admin/universities/{id}", universityUpdated, HttpMethod.Put);

            var universityLanguageEntity = await repo.GetUniversityLanguageByUniversityIdAsync(id);
            universityEntity = await repo.GetUniversityByIdAsync(id);

            
            //FIXME: removed
            //Assert.Equal(universityUpdated["website"], universityEntity.Website);
            Assert.Equal(universityUpdated["foundationYear"], universityEntity.FoundationYear);
            
            foreach (var universityLanguage in universityLanguageEntity)
            {
                var lang = await languageRepo.GetLanguageShortNameAsync(universityLanguage.Key);

                Assert.Equal(universityUpdated["languages"][lang]["htmlDescription"], universityLanguage.Value.Description);
                Assert.Equal(universityUpdated["languages"][lang]["url"], universityLanguage.Value.Url);
            }
        }

        public static IEnumerable<object[]> Data()
        {
            var university = UniversityDataSet.GetUniversity();

            var universityToUpdate = UniversityDataSet.GetUniversityWithChinaLang();

            return new List<object[]>
            {
                new object[]
                {
                    university,
                    "http://test.ru",
                    universityToUpdate
                },
            };
        }
    }
}
