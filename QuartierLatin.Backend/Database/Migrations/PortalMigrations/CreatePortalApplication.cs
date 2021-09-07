using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.PortalMigrations
{
    [MigrationDate(2021, 9, 8, 12, 15)]
    public class CreatePortalApplication : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("PortalApplications")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().ForeignKey("PortalUsers", "Id")
                .WithColumn("ApplicationStatus").AsInt16()
                .WithColumn("ApplicationType").AsInt16().Nullable()
                .WithColumn("CommonTypeSpecificApplicationInfo").AsCustom("jsonb")
                .WithColumn("EntityTypeSpecificApplicationInfo").AsCustom("jsonb");
        }
    }
}
