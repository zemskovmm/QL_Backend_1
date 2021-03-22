using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace QuartierLatin.Backend.Tests.GlobalSettingTests
{
    [Collection("GlobalSettingTests")]
    public class GlobalSettingGetData : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Get_GlobalSettingsAsync(string key, string lang, string expectedValue)
        {
            var response = SendAdminRequest<JObject>($"/api/global/{key}/{lang}", null);

            Assert.Equal(expectedValue, response["type"]);
        }

        public static IEnumerable<object[]> Data()
        {
            return new List<object[]>
            {
                new object[]
                {
                    "test1",
                    "en",
                    "text1"
                },
                new object[]
                {
                    "тест1",
                    "ru",
                    "текст"
                }
            };
        }
    }
}
