using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories;
using Xunit;

namespace QuartierLatin.Backend.Tests.PageTests
{
    public class PageTestAdminCreatePage : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Create_PageAsync(JObject pageEn, string expectedTitle)
        {
            var createReq = pageEn.DeepClone();
            createReq["language"] = "en";
            var resp = SendAdminRequest<JObject>("/api/admin/pages", new
            {
                languages = new
                {
                    en = pageEn
                }
            });
            var id = int.Parse(resp["id"].ToString());
            var repo = GetService<IPageRepository>();

            var pageEnEntity = await repo.GetPagesByPageRootIdAndLanguageIdAsync(id, LangIds["en"]);
            Assert.Equal(expectedTitle, pageEnEntity.Title);
        }

        public static IEnumerable<object[]> Data()
        {
            var pageEn = JObject.FromObject(new
            {
                url = "/test",
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
                    "test"
                },
            };
        }
    }
}
