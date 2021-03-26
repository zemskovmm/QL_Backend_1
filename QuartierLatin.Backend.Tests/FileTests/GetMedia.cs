using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Models.Repositories;
using QuartierLatin.Backend.Tests.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace QuartierLatin.Backend.Tests.FileTests
{
    public class GetMedia : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Anon_Should_Be_Able_To_Get_MediaAsync(string filePath, string fileName, string mediaType)
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

            var requestAnswer = SendAnonRequest<string>($"/media/{id}", null, null, null, true);

            var fileFromHdd = Convert.ToBase64String(await File.ReadAllBytesAsync(filePath));

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
                    "Image"
                },
            };
        }
    }
}
