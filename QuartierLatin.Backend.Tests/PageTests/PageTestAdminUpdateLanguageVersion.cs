using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace QuartierLatin.Backend.Tests.PageTests
{
    public class PageTestAdminUpdateLanguageVersion : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Create_And_Update_Language_VersionsAsync(JObject pageEn, string expectedTitle, string langShortName)
        {
            pageEn["language"] = langShortName;
            var resp = SendAdminRequest<JObject>("/api/admin/pages", new
            {
                languages = new Dictionary<string, object>()
                {
                    [langShortName] = pageEn
                }
            });
            var pageId = int.Parse(resp["id"].ToString());

            var repo = GetService<IPageRepository>();

            var pageEnEntity = await repo.GetPagesByPageRootIdAndLanguageIdAsync(pageId, LangIds[langShortName]);
            Assert.Equal(pageEnEntity.PageRootId, pageId);

            pageEn["title"] = expectedTitle;
            SendAdminRequest<object>($"/api/admin/pages/{pageId}", new
            {
                languages = new Dictionary<string, object>()
                {
                    [langShortName] = pageEn
                }
            }, HttpMethod.Put);

            pageEnEntity = await repo.GetPagesByPageRootIdAndLanguageIdAsync(pageId, LangIds[langShortName]);
            Assert.Equal(expectedTitle, pageEnEntity.Title);
        }

        public static IEnumerable<object[]> Data()
        {
            var pageEn = JObject.FromObject(new
            {
                url = "/test/123",
                title = "test",
                pageData = new
                {
                    blocks = new object[]
                    {
                        new
                        {
                            type = "text",
                            data = "Lorem ipsum dolor sit amet"
                        },
                        new
                        {
                            type = "standard-service-block",
                            data = new
                            {
                                generalInformation = "Lorem ipsum",
                                advantages = new[] {"Lorem", "ipsum"},
                                requirements = new[] {"Lorem", "ipsum"},
                                prices = new[] {"Lorem", "ipsum"}
                            }
                        }
                    },
                }
            });

            return new List<object[]>
            {
                new object[]
                {
                    pageEn,
                    "New title12",
                    "en"
                },
                new object[]
                {
                    pageEn,
                    "Заголовок12",
                    "ru"
                }
            };
        }
    }
}
