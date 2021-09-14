using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.StorageFoldersRepositories;
using QuartierLatin.Backend.Tests.StorageFoldersTests.StorageFolderDataSets;
using Xunit;

namespace QuartierLatin.Backend.Tests.StorageFoldersTests
{
    public class StorageFolderCreateTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_User_Should_Be_Able_To_Create_Storage_FolderAsync(JObject storageFolder, string title)
        {
            storageFolder["title"] = title;
            var resp = SendAdminRequest<JObject>("/api/media/directories", storageFolder);
            var id = int.Parse(resp["id"].ToString());
            var repo = GetService<IStorageFoldersRepository>();

            var storageFolderEntity = await repo.GetStorageFolderByIdAsync(id);

            Assert.Equal(storageFolder["title"], storageFolderEntity.FolderName);

        }

        public static IEnumerable<object[]> Data()
        {
            var storageFolder = StorageFolderDataSet.GetStorageFolder();

            return new List<object[]>
            {
                new object[]
                {
                    storageFolder,
                    "createTest",
                },
            };
        }
    }
}
