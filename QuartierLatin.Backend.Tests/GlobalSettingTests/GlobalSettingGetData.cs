using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Cache;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Storages.Cache;
using QuartierLatin.Backend.Tests.Infrastructure;
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
