using System.Linq;
using LinqToDB;
using QuartierLatin.Backend.Models;

namespace QuartierLatin.Backend.Database.AppDbContextSeed
{
    public static class AppDbContextLanguageSeed
    {
        public static void Seed(AppDbContextManager dbManager)
        {
            dbManager.Exec(db =>
            {
                if (!db.Languages.Any())
                {
                    db.Languages.Insert(() => new Language
                    {
                        LanguageName = "Русский",
                        LanguageShortName = "ru"
                    });

                    db.Languages.Insert(() => new Language
                    {
                        LanguageName = "en",
                        LanguageShortName = "English"
                    });

                    db.Languages.Insert(() => new Language
                    {
                        LanguageName = "fr",
                        LanguageShortName = "Français"
                    });

                    db.Languages.Insert(() => new Language
                    {
                        LanguageName = "cn",
                        LanguageShortName = "中文"
                    });

                    db.Languages.Insert(() => new Language
                    {
                        LanguageName = "es",
                        LanguageShortName = "Español"
                    });
                }
            });
            return;
        }
    }
}
