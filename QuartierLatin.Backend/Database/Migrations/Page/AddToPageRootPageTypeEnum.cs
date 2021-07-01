using FluentMigrator;
using QuartierLatin.Backend.Models.Enums;

namespace QuartierLatin.Backend.Database.Migrations.Page
{
    [MigrationDate(2021, 6, 27, 21, 20)]
    public class AddToPageRootPageTypeEnum : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("PageRoots")
                .AlterColumn("PageType").AsInt16().WithDefaultValue(PageType.Page);
        }
    }
}
