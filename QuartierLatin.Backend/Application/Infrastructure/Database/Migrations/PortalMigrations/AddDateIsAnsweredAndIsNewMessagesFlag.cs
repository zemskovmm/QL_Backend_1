using FluentMigrator;
using System;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.PortalMigrations
{
    [MigrationDate(2021, 10, 8, 12, 15)]
    public class AddDateIsAnsweredAndIsNewMessagesFlag : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("PortalApplications")
                .AlterColumn("Date").AsDateTime().WithDefaultValue(DateTime.Now)
                .AlterColumn("IsAnswered").AsBoolean().WithDefaultValue(false)
                .AlterColumn("IsNewMessages").AsBoolean().WithDefaultValue(false);
        }
    }
}
