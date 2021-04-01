﻿using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto.UniversityDto.GetUniversityListDto;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Models.Repositories.CatalogRepositoies;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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
            Assert.Equal(expectedTitle, universityEntity.Website);

            SendAdminRequest<UniversityListDto>($"/api/admin/universities/{id}", universityUpdated, HttpMethod.Put);

            var universityLanguageEntity = await repo.GetUniversityLanguageByUniversityIdAsync(id);
            universityEntity = await repo.GetUniversityByIdAsync(id);

            Assert.Equal(universityUpdated["website"], universityEntity.Website);
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
            var university = JObject.FromObject(new
            {
                website = "/test",
                foundationYear = 1999,
                languages = new
                {
                    ru = new
                    {
                        name = "тест",
                        htmlDescription = "Видишь тест. И я его не вижу, а он есть",
                        url = ""
                    },
                    en = new
                    {
                        name = "text",
                        htmlDescription = "Lorem ipsum dolor sit amet",
                        url = ""
                    },
                }
            });

            var universityToUpdate = JObject.FromObject(new
            {
                website = "/test",
                foundationYear = 1998,
                languages = new
                {
                    ru = new
                    {
                        name = "тест",
                        htmlDescription = "Видишь тест. И я его не вижу, а он есть",
                        url = ""
                    },
                    en = new
                    {
                        name = "text",
                        htmlDescription = "Lorem ipsum dolor sit amet",
                        url = ""
                    },
                    cn = new
                    {
                        name = "text",
                        htmlDescription = "Lorem ipsum dolor sit amet",
                        url = ""
                    },
                }
            });

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
