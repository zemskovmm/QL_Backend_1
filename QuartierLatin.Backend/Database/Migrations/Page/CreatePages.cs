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
                .WithColumn("Url").AsString()
                .WithColumn("Title").AsString()
                .WithColumn("PageData").AsCustom("jsonb")
                .WithColumn("LanguageId").AsInt64().ForeignKey("Languages", "Id").PrimaryKey()
                .WithColumn("PageRootId").AsInt64().ForeignKey("PageRoots", "Id").PrimaryKey();
        }
    }
}
