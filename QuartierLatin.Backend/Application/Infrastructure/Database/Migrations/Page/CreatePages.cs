using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.Page
{
    [MigrationDate(2021, 3, 14, 23, 10)]
    public class CreatePages : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("PageRoots")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity();

            Create.Table("Pages")
                .WithColumn("Url").AsString()
                .WithColumn("Title").AsString()
                .WithColumn("PageData").AsCustom("jsonb")
                .WithColumn("LanguageId").AsInt32().ForeignKey("Languages", "Id").PrimaryKey()
                .WithColumn("PageRootId").AsInt32().ForeignKey("PageRoots", "Id").PrimaryKey();
        }
    }
}
