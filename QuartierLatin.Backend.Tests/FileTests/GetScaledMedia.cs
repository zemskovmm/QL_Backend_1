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
using QuartierLatin.Backend.Utils;
using Xunit;

namespace QuartierLatin.Backend.Tests.FileTests
{
    public class GetScaledMedia : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Anon_Should_Be_Able_To_Get_Scaled_MediaAsync(string fileName, string mediaType,
                int mediaDimension)
        {
            var filePath = Path.Combine(GetAssetPath(), this.GetType().Name, MethodBase.GetCurrentMethod().GetDeclaringName(), fileName);
            var repo = GetService<IBlobRepository>();

            var multipartContent = new MultipartFormDataContent();

            var fileStream = File.OpenRead(filePath);
            multipartContent.Add(new StreamContent(fileStream), "UploadedFile", fileName);
            multipartContent.Add(new StringContent(mediaType), "FileType");

            var resp = SendAdminRequest<JObject>("/media", multipartContent);
            var id = int.Parse(resp["id"].ToString());
            var entity = await repo.GetBlobInfoAsync(id);

            Assert.Equal(entity.Id, id);

            await using var stream = new MemoryStream(await File.ReadAllBytesAsync(filePath));
            await using var output = new MemoryStream();

            var imageScaler = new ImageScaler(mediaDimension);

            imageScaler.Scale(stream, output);

            var requestAnswer =
                SendAnonRequest<string>($"/media/scaled/{id}?dimension={mediaDimension}", null, null, null, true);

            var fileFromHdd = Convert.ToBase64String(output.ToArray());

            Assert.Equal(fileFromHdd, requestAnswer);

            var secondRequestAnswer =
                SendAnonRequest<string>($"/media/scaled/{id}?dimension={mediaDimension}", null, null, null, true);

            Assert.Equal(requestAnswer, secondRequestAnswer);
        }

        public static IEnumerable<object[]> Data()
        {
            var fileName = "test2.jpg";

            return new List<object[]>
                {
                    new object[]
                    {
                        fileName,
                        "Image",
                        150
                    },
                };
        }
    }
}
