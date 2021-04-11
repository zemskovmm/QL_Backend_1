using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.CommonTraits
{
    [MigrationDate(2021, 3, 29, 20, 30)]
    public class CreateCommonTraitTypesForEntites : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("CommonTraitTypesForEntites")
                .WithColumn("CommonTraitId").AsInt32().ForeignKey("CommonTraitTypes", "Id").PrimaryKey()
                .WithColumn("EntityType").AsInt32();
        }
    }
}
