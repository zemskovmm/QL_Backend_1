using LinqToDB;
using QuartierLatin.Backend.Models.ImageStandardSizeModels;
using System.Linq;

namespace QuartierLatin.Backend.Database.AppDbContextSeed
{
    public class AppDbContextImageStandardSizeSeed
    {
        public static void Seed(AppDbContextManager dbManager)
        {
            dbManager.Exec(db =>
            {
                if (!db.ImageStandardSizes.Any())
                {
                    db.ImageStandardSizes.InsertWithInt32IdentityAsync(() => new ImageStandardSize
                    {
                        Width = 340,
                        Height = 340
                    });

                    db.ImageStandardSizes.InsertWithInt32IdentityAsync(() => new ImageStandardSize
                    {
                        Width = 100,
                        Height = 100
                    });

                    db.ImageStandardSizes.InsertWithInt32IdentityAsync(() => new ImageStandardSize
                    {
                        Width = 1140,
                        Height = 300
                    });
                }
            });
            return;
        }
    }
}
