using System.Linq;
using LinqToDB;
using QuartierLatin.Backend.Auth;
using QuartierLatin.Backend.Models;

namespace QuartierLatin.Backend.Database.AppDbContextSeed
{
    public static class AppDbContextUserSeed
    {
        public static void Seed(AppDbContextManager dbManager)
        {
            dbManager.Exec(db =>
            {
                if (!db.Users.Any())
                    db.Users.Insert(() => new User
                    {
                        Email = "user@example.com",
                        PasswordHash = PasswordToolkit.EncodeSshaPassword("123321"),
                        Confirmed = true
                    });
            });
            return;
        }
    }
}
