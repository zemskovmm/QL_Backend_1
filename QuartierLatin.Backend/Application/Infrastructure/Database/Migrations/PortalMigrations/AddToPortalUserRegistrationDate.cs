using FluentMigrator;
using System;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.PortalMigrations
{
    [MigrationDate(2021, 10, 11, 12, 15)]
    public class AddToPortalUserRegistrationDate : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("PortalApplications")
                .AlterColumn("RegistrationDate").AsDateTime().WithDefaultValue(DateTime.Now);
        }
    }
}
