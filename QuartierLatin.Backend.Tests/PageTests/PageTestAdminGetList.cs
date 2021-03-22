using QuartierLatin.Backend.Dto.AdminPageModuleDto;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QuartierLatin.Backend.Tests.PageTests
{
    [Collection("PageAdminTests")]
    public class PageTestAdminGetList : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Get_List_Pages_Async(int page, string search, int pageId)
        {
            var result = SendAdminRequest<PageListDto>($"/api/admin/pages/?page={page}&search={search}", null);

            var id = result.Results.FirstOrDefault(page => page.Titles.ContainsValue(search)).Id;

            Assert.Equal(pageId, id);
        }

        public static IEnumerable<object[]> Data()
        {
            return new List<object[]>
            {
                new object[]
                {
                    1,
                    "test",
                    1
                }
            };
        }
    }
}
