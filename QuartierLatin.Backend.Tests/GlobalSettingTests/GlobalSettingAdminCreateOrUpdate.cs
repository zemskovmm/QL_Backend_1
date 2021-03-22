using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Cache;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Storages.Cache;
using QuartierLatin.Backend.Tests.Infrastructure;
using Xunit;

namespace QuartierLatin.Backend.Tests.GlobalSettingTests
{
    public class GlobalSettingAdminCreateOrUpdate : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Create_And_Update_GlobalSettingsAsync(JObject jsonData, string key, string lang, string expectedValue)
        {
            var cache = GetService<GlobalSettingsCache<JObject>>();
            var languageRepository = GetService<ILanguageRepository>();

            jsonData["type"] = expectedValue;

            SendAdminRequest<object>($"/api/admin/global/{key}/{lang}", jsonData, HttpMethod.Put);

            var languageId = await languageRepository.GetLanguageIdByShortNameAsync(lang);
            var cacheKey = new GlobalSettingsCacheKey(key, languageId);

            var globalSettingEntity = (JObject)await cache.GetCachedDataAsync(cacheKey);

            Assert.Equal(expectedValue, globalSettingEntity["type"]);
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
                    "test1",
                    "en",
                    "text"
                },
                new object[]
                {
                    globalSettingData,
                    "тест1",
                    "ru",
                    "текст"
                },
                new object[]
                {
                    globalSettingData,
                    "test1",
                    "en",
                    "text1"
                },
            };
        }
    }
}
