namespace QuartierLatin.Backend.Application.Infrastructure.Database.AppDbContextSeed
{
    public static class AppDbContextSeed
    {
        public static void Seed(AppDbContextManager dbManager)
        {
            AppDbContextUserSeed.Seed(dbManager);
            AppDbContextLanguageSeed.Seed(dbManager);
            AppDbContextAppStateEntrySeed.Seed(dbManager);
            AppDbContextImageStandardSizeSeed.Seed(dbManager);
        }
    }
}
