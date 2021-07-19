using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.CommonTraits
{
    [MigrationDate(2021, 6, 27, 20, 20)]
    public class CreateCommonTraitsToPages : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("CreateCommonTraitsToPages")
                .WithColumn("PageId").AsInt32().ForeignKey("PageRoots", "Id").PrimaryKey()
                .WithColumn("CommonTraitId").AsInt32().ForeignKey("CommonTraits", "Id").PrimaryKey();
        }
    }
}
