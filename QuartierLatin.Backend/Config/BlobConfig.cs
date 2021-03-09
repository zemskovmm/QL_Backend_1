namespace QuartierLatin.Backend.Config
{
    public enum BlobTypes
    {
        Azure,
        Local
    }

    public class AzureBlobConfig
    {
        public string ConnectionString { get; set; } =
            "DefaultEndpointsProtocol=http;" + "AccountName=devstoreaccount1;" +
            "AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;" +
            "BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;" +
            "QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;";

        public string Container { get; set; } = "turbine";
    }

    public class LocalBlobConfig
    {
        public string Path { get; set; }
    }

    public class BlobConfig
    {
        public BlobTypes Type { get; set; } = BlobTypes.Azure;
        public AzureBlobConfig Azure { get; set; } = new();
        public LocalBlobConfig Local { get; set; } = new();
    }
}