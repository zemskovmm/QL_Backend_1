namespace QuartierLatin.Backend.Models.Repositories
{
    public interface IBlobRepository
    {
        long CreateBlobId();
        void DeleteBlob(long id);
    }
}