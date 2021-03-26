using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Tests.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using QuartierLatin.Backend.Utils;
using Xunit;

namespace QuartierLatin.Backend.Tests.FileTests
{
    public class GetScaledMediaAsync : TestBase
    {
        public class GetMedia : TestBase
        {
            [Theory]
            [MemberData(nameof(Data))]
            public async Task Anon_Should_Be_Able_To_Get_MediaAsync(string filePath, string fileName, string mediaType,
                int mediaDimension)
            {
                var repo = GetService<IBlobRepository>();

                var multipartContent = new MultipartFormDataContent();

                var fileStream = File.OpenRead(filePath);
                multipartContent.Add(new StreamContent(fileStream), "UploadedFile", fileName);
                multipartContent.Add(new StringContent(mediaType), "FileType");

                var resp = SendAdminRequest<JObject>("/media", multipartContent);
                var id = int.Parse(resp["id"].ToString());
                var entity = await repo.GetBlobInfoByFileNameAndFileTypeAsync(fileName, mediaType);

                Assert.Equal(entity.Id, id);

                await using var stream = new MemoryStream(await File.ReadAllBytesAsync(filePath));
                await using var output = new MemoryStream();

                var imageScaler = new ImageScaler(mediaDimension);

                imageScaler.Scale(stream, output);

                var requestAnswer =
                    SendAnonRequest<string>($"/media/scaled/{id}?dimension={mediaDimension}", null, null, null, true);

                var fileFromHdd = Convert.ToBase64String(output.ToArray());

                Assert.Equal(fileFromHdd, requestAnswer);
            }

            public static IEnumerable<object[]> Data()
            {
                var fileName = "test.jpg";
                var enviroment = System.Environment.CurrentDirectory;
                var projectDirectory = Directory.GetParent(enviroment).Parent.Parent.FullName;

                var filePath = Path.Combine(projectDirectory, @"TestData\" + fileName);

                return new List<object[]>
                {
                    new object[]
                    {
                        filePath,
                        fileName,
                        "Image",
                        150
                    },
                };
            }
        }
    }
}
