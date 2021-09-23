using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.HousingMigrations
{
    [MigrationDate(2021, 9, 20, 16, 30)]
    public class AddToHousingLocationJObject : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("HousingLanguages")
                .AddColumn("Location").AsCustom("jsonb").Nullable();
        }
    }
}
