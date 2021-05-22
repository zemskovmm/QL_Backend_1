using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Tests.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using QuartierLatin.Backend.Tests.Extensions;
using Xunit;

namespace QuartierLatin.Backend.Tests.FileTests
{
    public class GetMedia : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Anon_Should_Be_Able_To_Get_MediaAsync(string fileName, string mediaType)
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

            Assert.Equal(entity.Id, id);

            var requestAnswer = SendAnonRequest<string>($"/api/media/{id}", null, null, null, true);

            var fileFromHdd = Convert.ToBase64String(await File.ReadAllBytesAsync(filePath));

            Assert.Equal(fileFromHdd, requestAnswer);
        }

        public static IEnumerable<object[]> Data()
        {
            var fileName = "test1.jpg";

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
