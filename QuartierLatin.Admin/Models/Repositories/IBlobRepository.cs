namespace QuartierLatin.Admin.Models.Repositories
{
    public interface IBlobRepository
    {
        long CreateBlobId();
        void DeleteBlob(long id);
    }
}