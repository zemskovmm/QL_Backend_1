using Newtonsoft.Json.Linq;
using QuartierLatin.Backend.Dto.StorageFoldersDto;
using QuartierLatin.Backend.Tests.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Repositories.StorageFoldersRepositories;
using QuartierLatin.Backend.Tests.StorageFoldersTests.StorageFolderDataSets;
using Xunit;

namespace QuartierLatin.Backend.Tests.StorageFoldersTests
{
    public class StorageFolderGetDefaultTest : TestBase
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task Admin_User_Should_Be_Able_To_Get_Default_Storage_FolderAsync(JObject storageFolder, string title)
        {
            storageFolder["title"] = title;
            var resp = SendAdminRequest<JObject>("/api/media/directories", storageFolder);
            var id = int.Parse(resp["id"].ToString());
            var repo = GetService<IStorageFoldersRepository>();

            var storageFolderEntity = await repo.GetStorageFolderByIdAsync(id);

            Assert.Equal(storageFolder["title"], storageFolderEntity.FolderName);

            var responseStorageFolders = SendAdminRequest<StorageFolderDtoAdmin>("/api/media/directories", null);

            Assert.Equal(storageFolder["title"], responseStorageFolders.Directories
                .FirstOrDefault(folder => folder.Id == id)?.DirectoryName);
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
