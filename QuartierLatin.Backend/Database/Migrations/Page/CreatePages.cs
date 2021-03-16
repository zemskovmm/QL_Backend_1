using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.Page
{
    [MigrationDate(2021, 3, 14, 23, 10)]
    public class CreatePages : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Pages")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Url").AsString().NotNullable()
                .WithColumn("Title").AsString().NotNullable()
                .WithColumn("LanguageId").AsInt64().NotNullable()
                .WithColumn("PageRootId").AsInt64().NotNullable();
        }
    }
}
