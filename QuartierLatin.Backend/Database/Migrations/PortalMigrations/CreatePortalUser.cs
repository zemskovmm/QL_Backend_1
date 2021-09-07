using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.PortalMigrations
{
    [MigrationDate(2021, 9, 8, 12, 10)]
    public class CreatePortalUser : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("PortalUsers")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Email").AsString()
                .WithColumn("Phone").AsString()
                .WithColumn("FirstName").AsString()
                .WithColumn("LastName").AsString()
                .WithColumn("PasswordHash").AsString()
                .WithColumn("PersonalInfo").AsCustom("jsonb");
        }
    }
}
