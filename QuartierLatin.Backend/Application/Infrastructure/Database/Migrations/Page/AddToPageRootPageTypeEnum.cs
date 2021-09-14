using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.Page
{
    [MigrationDate(2021, 6, 27, 21, 20)]
    public class AddToPageRootPageTypeEnum : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("PageRoots")
                .AddColumn("PageType").AsInt16().WithDefaultValue(0);
        }
    }
}
