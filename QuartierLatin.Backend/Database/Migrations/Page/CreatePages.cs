using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.Page
{
    [MigrationDate(2021, 3, 14, 23, 10)]
    public class CreatePages : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("PageRoots")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity();

            Create.Table("Pages")
                .WithColumn("Url").AsString().NotNullable()
                .WithColumn("Title").AsString().NotNullable()
                .WithColumn("PageData").AsCustom("jsonb").Nullable()
                .WithColumn("LanguageId").AsInt64().ForeignKey("Languages", "Id").PrimaryKey().Identity()
                .WithColumn("PageRootId").AsInt64().ForeignKey("PageRoots", "Id").PrimaryKey().Identity();
        }
    }
}
