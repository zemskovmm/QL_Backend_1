namespace QuartierLatin.Backend.Database.AppDbContextSeed
{
    public static class AppDbContextSeed
    {
        public static void Seed(AppDbContextManager dbManager)
        {
            AppDbContextUserSeed.Seed(dbManager);
            AppDbContextLanguageSeed.Seed(dbManager);
        }
    }
}
