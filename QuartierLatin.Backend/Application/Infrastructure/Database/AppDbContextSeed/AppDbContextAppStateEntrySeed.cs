using System;
using System.Linq;
using LinqToDB;
using QuartierLatin.Backend.Application.ApplicationCore.Models.AppStateModels;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.AppDbContextSeed
{
    public static class AppDbContextAppStateEntrySeed
    {
        public static void Seed(AppDbContextManager dbManager)
        {
            dbManager.Exec(db =>
            {
                if (!db.AppStateEntries.Any())
                {
                    db.AppStateEntries.Insert(() => new AppStateEntry
                    {
                        Key = "LastUpdate",
                        Value = DateTime.Now.AddYears(-2).ToString()
                    });

                    db.AppStateEntries.Insert(() => new AppStateEntry
                    {
                        Key = "LastChange",
                        Value = DateTime.Now.AddYears(-1).ToString()
                    });
                }
            });
            return;
        }
    }
}
