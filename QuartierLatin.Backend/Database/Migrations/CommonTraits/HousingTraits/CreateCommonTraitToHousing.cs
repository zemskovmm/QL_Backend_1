using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.CommonTraits.HousingTraits
{
    [MigrationDate(2021, 7, 28, 12, 20)]
    public class CreateCommonTraitToHousing : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("CommonTraitsToHousing")
                .WithColumn("HousingId").AsInt32().ForeignKey("Housings", "Id").PrimaryKey()
                .WithColumn("CommonTraitId").AsInt32().ForeignKey("CommonTraits", "Id").PrimaryKey();
        }
    }
}
