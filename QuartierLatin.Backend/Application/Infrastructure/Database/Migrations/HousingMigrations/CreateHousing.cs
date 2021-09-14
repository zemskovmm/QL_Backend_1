using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.HousingMigrations
{
    [MigrationDate(2021, 7, 28, 12, 10)]
    public class CreateHousing : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Housings")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Price").AsInt32().Nullable();

            Create.Table("HousingLanguages")
                .WithColumn("HousingId").AsInt32().ForeignKey("Housings", "Id").PrimaryKey()
                .WithColumn("LanguageId").AsInt32().ForeignKey("Languages", "Id").PrimaryKey()
                .WithColumn("Name").AsString()
                .WithColumn("Description").AsString()
                .WithColumn("Url").AsString()
                .WithColumn("Metadata").AsCustom("jsonb").Nullable();
        }
    }
}
