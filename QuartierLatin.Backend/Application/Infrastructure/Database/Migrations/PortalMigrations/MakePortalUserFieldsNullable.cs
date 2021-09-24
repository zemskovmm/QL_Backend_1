using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.PortalMigrations
{
    [MigrationDate(2021, 9, 22, 12, 10)]
    public class MakePortalUserFieldsNullable : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("PortalUsers")
                .AlterColumn("Phone").AsString().Nullable()
                .AlterColumn("FirstName").AsString().Nullable()
                .AlterColumn("LastName").AsString().Nullable()
                .AlterColumn("PersonalInfo").AsCustom("jsonb").Nullable();
        }
    }
}
