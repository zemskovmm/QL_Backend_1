using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.GlobalSettings
{
    [MigrationDate(2021, 3, 14, 23, 40)]
    public class CreateGlobalSettings : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("GlobalSettings")
                .WithColumn("Key").AsString().PrimaryKey()
                .WithColumn("LanguageId").AsInt32().ForeignKey("Languages", "Id").PrimaryKey()
                .WithColumn("JsonData").AsCustom("jsonb");
        }
    }
}
