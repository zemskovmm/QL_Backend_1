using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto.AdminPageModuleDto;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QuartierLatin.Backend.Tests.PageTests
{
    public class PageTestAdminGetList : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Create_And_Get_List_Async(JObject pageEn, int page, string search, int pageId, string newTitle, string langShortName)
        {
            pageEn["language"] = langShortName;
            pageEn["title"] = newTitle;

            var resp = SendAdminRequest<JObject>("/api/admin/pages", new
            {
                languages = new
                {
                    en = pageEn
                }
            });
            
            var id = int.Parse(resp["id"].ToString());

            var repo = GetService<IPageRepository>();

            var pageEnEntity = await repo.GetPagesByPageRootIdAndLanguageIdAsync(id, LangIds[langShortName]);
            Assert.Equal(newTitle, pageEnEntity.Title);

            var result = SendAdminRequest<PageListDto>($"/api/admin/pages/?page={page}&search={search}", null);

            var responseId = result.Results.FirstOrDefault(page => page.Titles.ContainsValue(search)).Id;

            Assert.Equal(id, responseId);
        }

        public static IEnumerable<object[]> Data()
        {
            var pageEn = JObject.FromObject(new
            {
                url = "/test/3434",
                title = "test343434",
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
                    1,
                    "test9789",
                    1,
                    "test9789",
                    "en"
                }
            };
        }
    }
}
