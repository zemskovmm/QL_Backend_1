using Newtonsoft.Json.Linq;

namespace QuartierLatin.Backend.Tests.StorageFoldersTests.StorageFolderDataSets
{
    public static class StorageFolderDataSet
    {
        public static JObject GetStorageFolder(int? parentId = null)
        {
            return JObject.FromObject(new
            {
                title = "My documents",
                parentId = parentId
            });
        }
    }
}
