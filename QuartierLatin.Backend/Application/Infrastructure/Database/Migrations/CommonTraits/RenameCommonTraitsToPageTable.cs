using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.CommonTraits
{
    [MigrationDate(2021, 7, 27, 10, 20)]
    public class RenameCommonTraitsToPageTable : AutoReversingMigration
    {
        public override void Up()
        {
            Rename.Table("CreateCommonTraitsToPages").To("CommonTraitsToPages");
        }
    }
}
