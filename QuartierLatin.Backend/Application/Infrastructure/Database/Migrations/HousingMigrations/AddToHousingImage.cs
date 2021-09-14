using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.HousingMigrations
{
    [MigrationDate(2021, 8, 30, 16, 30)]
    public class AddToHousingImage : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("Housings")
                .AddColumn("ImageId").AsInt32().ForeignKey("Blobs", "Id").Nullable();
        }
    }
}
