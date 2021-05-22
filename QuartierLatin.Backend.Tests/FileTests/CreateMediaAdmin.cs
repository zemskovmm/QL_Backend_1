using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Tests.Extensions;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace QuartierLatin.Backend.Tests.FileTests
{
    public class CreateMediaAdmin : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_Should_Be_Able_To_Create_MediaAsync(string fileName, string mediaType)
        {
            var filePath = Path.Combine(GetAssetPath(), this.GetType().Name, MethodBase.GetCurrentMethod().GetDeclaringName(), fileName);

            var repo = GetService<IBlobRepository>();
            var multipartContent = new MultipartFormDataContent();

            var fileStream = File.OpenRead(filePath);
            multipartContent.Add(new StreamContent(fileStream), "UploadedFile", fileName);
            multipartContent.Add(new StringContent(mediaType), "FileType");

            var resp = SendAdminRequest<JObject>("/api/media", multipartContent);
            var id = int.Parse(resp["id"].ToString());
            var entity = await repo.GetBlobInfoAsync(id);

            Assert.Equal(entity.Id,id);
        }

        public static IEnumerable<object[]> Data()
        {
            var fileName = "test.jpg";
            
            return new List<object[]>
            {
                new object[]
                {
                    fileName,
                    "Image"
                },
            };
        }
    }
}
