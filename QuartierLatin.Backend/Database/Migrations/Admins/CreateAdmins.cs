using System;
using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.Users
{
    [MigrationDate(2020, 10, 6, 12, 48)]
    public class CreateAdmins : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Admins")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Email").AsString().Nullable().Unique()
                .WithColumn("PasswordHash").AsString().Nullable();

            Alter.Table("Admins").AddColumn("AvatarImage").AsInt32().Nullable();

            Alter.Table("Admins")
                .AddColumn("Confirmed").AsBoolean().SetExistingRowsTo(false).WithDefaultValue(false)
                .AddColumn("ConfirmationCode").AsString().Nullable();

            Alter.Table("Admins")
                .AddColumn("AzureIdentityId").AsGuid().SetExistingRowsTo(Guid.Empty)
                .AddColumn("Name").AsString().SetExistingRowsTo("Unknown User");
        }
    }
}