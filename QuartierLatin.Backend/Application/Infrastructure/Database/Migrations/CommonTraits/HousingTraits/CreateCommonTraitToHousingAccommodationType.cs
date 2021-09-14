using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.CommonTraits.HousingTraits
{
    [MigrationDate(2021, 7, 28, 12, 25)]
    public class CreateCommonTraitToHousingAccommodationType : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("CommonTraitsToHousingAccommodationType")
                .WithColumn("HousingAccommodationTypeId").AsInt32().ForeignKey("HousingAccommodationTypes", "Id").PrimaryKey()
                .WithColumn("CommonTraitId").AsInt32().ForeignKey("CommonTraits", "Id").PrimaryKey();
        }
    }
}
