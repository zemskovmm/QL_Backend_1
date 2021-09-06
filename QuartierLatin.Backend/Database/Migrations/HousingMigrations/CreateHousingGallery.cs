using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.HousingMigrations
{
    [MigrationDate(2021, 8, 23, 11, 20)]
    public class CreateHousingGallery : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("HousingGalleries")
                .WithColumn("HousingId").AsInt32().ForeignKey("Housings", "Id").PrimaryKey()
                .WithColumn("ImageId").AsInt32().ForeignKey("Blobs", "Id").PrimaryKey();
        }
    }
}
