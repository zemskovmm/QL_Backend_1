using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.Language
{
    [MigrationDate(2021, 3, 14, 22, 30)]
    public class CreateLanguages : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Languages")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("LanguageName").AsString().NotNullable()
                .WithColumn("LanguageShortName").AsString().NotNullable();
        }
    }
}
