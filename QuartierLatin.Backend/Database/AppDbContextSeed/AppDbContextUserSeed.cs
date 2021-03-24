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
                if (!db.Admins.Any())
                {
                    db.Admins.Insert(() => new Admin
                    {
                        Email = "user@example.com",
                        PasswordHash = PasswordToolkit.EncodeSshaPassword("123321"),
                        Confirmed = true
                    });

                    db.Insert(new AdminRole()
                    {
                        Role = "Admin",
                        AdminId = 1
                    });
                }
            });
            return;
        }
    }
}
