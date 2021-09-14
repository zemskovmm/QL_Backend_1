using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using QuartierLatin.Backend.Application.ApplicationCore.Models.Cache;
using QuartierLatin.Backend.Storages.Cache;
using Xunit;

namespace QuartierLatin.Backend.Tests.GlobalSettingTests
{
    public class GlobalSettingGetData : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Should_Be_Able_To_Set_Global_Data_And_Read_It_With_Normal_Api(JObject jsonData, string key, string lang, string expectedValue)
        {
            var cache = GetService<GlobalSettingsCache<JObject>>();
            var languageRepository = GetService<ILanguageRepository>();

            jsonData["type"] = expectedValue;

            SendAdminRequest<object>($"/api/admin/global/{key}/{lang}", jsonData, HttpMethod.Put);

            var response = SendAdminRequest<JObject>($"/api/global/{key}/{lang}", null);

            var languageId = await languageRepository.GetLanguageIdByShortNameAsync(lang);
            var cacheKey = new GlobalSettingsCacheKey(key, languageId);

            var responseFromCache = (JObject)await cache.GetCachedDataAsync(cacheKey);

            Assert.Equal(responseFromCache["type"], response["type"]);
        }

        public static IEnumerable<object[]> Data()
        {
            var globalSettingData = JObject.FromObject(new
            {
                jsonData = new
                {
                    type = "text",
                    data = "Lorem ipsum dolor sit amet"
                }
            });

            return new List<object[]>
            {
                new object[]
                {
                    globalSettingData,
                    "test12",
                    "en",
                    "text1"
                },
                new object[]
                {
                    globalSettingData,
                    "тест12",
                    "ru",
                    "текст"
                },
                new object[]
                {
                    globalSettingData,
                    "test21",
                    "en",
                    "text2"
                }
            };
        }
    }
}
