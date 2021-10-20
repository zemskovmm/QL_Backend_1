using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.PortalMigrations
{
    [MigrationDate(2021, 10, 15, 11, 15)]
    public class AddPortalApplicationFileStorage : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("PortalApplicationFileStorages")
                .WithColumn("ApplicationId").AsInt32().ForeignKey("PortalApplications", "Id").PrimaryKey()
                .WithColumn("BlobId").AsInt32().ForeignKey("Blobs", "Id").PrimaryKey();
        }
    }
}
