using System;
using FluentMigrator;

namespace QuartierLatin.Admin.Database.Migrations.Users
{
    [MigrationDate(2020, 10, 6, 12, 48)]
    public class CreateUsers : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Email").AsString().Nullable().Unique()
                .WithColumn("PasswordHash").AsString().Nullable();

            Alter.Table("Users").AddColumn("AvatarImage").AsInt64().Nullable();

            Alter.Table("Users")
                .AddColumn("Confirmed").AsBoolean().SetExistingRowsTo(false).WithDefaultValue(false)
                .AddColumn("ConfirmationCode").AsString().Nullable();

            Alter.Table("Users")
                .AddColumn("AzureIdentityId").AsGuid().SetExistingRowsTo(Guid.Empty)
                .AddColumn("Name").AsString().SetExistingRowsTo("Unknown User");
        }
    }
}