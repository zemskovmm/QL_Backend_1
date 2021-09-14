using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto.StorageFoldersDto;
using QuartierLatin.Backend.Tests.Infrastructure;
using QuartierLatin.Backend.Tests.StorageFoldersTests.StorageFolderDataSets;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.StorageFoldersRepositories;
using Xunit;

namespace QuartierLatin.Backend.Tests.StorageFoldersTests
{
    public class StorageFolderRenameAndDeleteTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_User_Should_Be_Able_To_Rename_And_Delete_Storage_FolderAsync(JObject storageFolder, string title, JObject newTitle)
        {
            storageFolder["title"] = title;
            var resp = SendAdminRequest<JObject>("/api/media/directories", storageFolder);
            var id = int.Parse(resp["id"].ToString());
            var repo = GetService<IStorageFoldersRepository>();

            var storageFolderEntity = await repo.GetStorageFolderByIdAsync(id);

            Assert.Equal(storageFolder["title"], storageFolderEntity.FolderName);

            var responseStorageFolders = SendAdminRequest<StorageFolderDtoAdmin>($"/api/media/directories/{id}", null);

            Assert.Equal(storageFolder["title"], responseStorageFolders.FolderName);

            SendAdminRequest<JObject>($"/api/media/directories/{id}", newTitle, HttpMethod.Patch);

            storageFolderEntity = await repo.GetStorageFolderByIdAsync(id);

            Assert.Equal(newTitle["title"], storageFolderEntity.FolderName);

            SendAdminRequest<JObject>($"/api/media/directories/{id}", null, HttpMethod.Delete);

            storageFolderEntity = await repo.GetStorageFolderByIdAsync(id);

            Assert.Null(storageFolderEntity);
        }

        public static IEnumerable<object[]> Data()
        {
            var storageFolder = StorageFolderDataSet.GetStorageFolder();

            var newTitle = JObject.FromObject(new
            {
                title = "NewTitleForTest"
            });
            
            return new List<object[]>
            {
                new object[]
                {
                    storageFolder,
                    "createTest",
                    newTitle
                },
            };
        }
    }
}

