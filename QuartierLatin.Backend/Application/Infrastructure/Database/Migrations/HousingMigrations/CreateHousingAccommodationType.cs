using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.HousingMigrations
{
    [MigrationDate(2021, 7, 28, 12, 15)]
    public class CreateHousingAccommodationType : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("HousingAccommodationTypes")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Names").AsCustom("jsonb")
                .WithColumn("Square").AsString()
                .WithColumn("Residents").AsString()
                .WithColumn("Price").AsInt32()
                .WithColumn("HousingId").AsInt32().ForeignKey("Housings", "Id");
        }
    }
}
