using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using QuartierLatin.Backend.Config;
using Microsoft.Extensions.Options;

namespace QuartierLatin.Backend.Storages
{
    public class AzureBlobStorage : IBlobFileStorage
    {
        public AzureBlobStorage(BlobServiceClient client)
        {
            Client = client;
        }

        public AzureBlobStorage(IOptions<BlobConfig> config)
        {
            var configAzure = config.Value.Azure;
            Client = new BlobServiceClient(configAzure.ConnectionString);
            BlobContainer = Client.GetBlobContainerClient(configAzure.Container);
            if (!BlobContainer.Exists()) BlobContainer = Client.CreateBlobContainer(configAzure.Container);
        }

        private BlobServiceClient Client { get; }
        private BlobContainerClient BlobContainer { get; }

        public async Task CreateBlobAsync(long id, Stream s, int? dimension)
        {
            var blob = BlobContainer.GetBlobClient($"{id}");
            await blob.UploadAsync(s);
        }

        public Stream OpenBlob(long id, int? dimension)
        {
            var blob = BlobContainer.GetBlobClient($"{id}");
            return blob.OpenRead();
        }

        public async Task DeleteBlob(long id, int? dimension)
        {
            var blob = BlobContainer.GetBlobClient($"{id}");
            await blob.DeleteIfExistsAsync();
        }

        public bool CheckIfExist(long id, int? dimension = null)
        {
            var blob = BlobContainer.GetBlobClient($"{id}");
            return blob.Exists();
        }
    }
}