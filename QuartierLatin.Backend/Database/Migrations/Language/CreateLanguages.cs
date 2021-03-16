using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.Language
{
    [MigrationDate(2021, 3, 14, 23, 30)]
    public class CreateLanguages : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Languages")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("LanguageName").AsString().NotNullable()
                .WithColumn("LanguageShortName").AsString().NotNullable();
        }
    }
}
