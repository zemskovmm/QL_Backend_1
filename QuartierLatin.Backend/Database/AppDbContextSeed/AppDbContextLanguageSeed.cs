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
                        LanguageName = "English",
                        LanguageShortName = "en"
                    });

                    db.Languages.Insert(() => new Language
                    {
                        LanguageName = "Français",
                        LanguageShortName = "fr"
                    });

                    db.Languages.Insert(() => new Language
                    {
                        LanguageName = "Español",
                        LanguageShortName = "esp"
                    });
                    
                    db.Languages.Insert(() => new Language
                    {
                        LanguageName = "中文",
                        LanguageShortName = "ch"
                    });
                }
            });
            return;
        }
    }
}
