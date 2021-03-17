using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.Roles
{
    [MigrationDate(2020, 10, 22, 12, 48)]
    public class CreateAdminRoles : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("AdminRole")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("Role").AsString().NotNullable();

            Create.ForeignKey().FromTable("AdminRole").ForeignColumn("AdminId").ToTable("Admins").PrimaryColumn("Id");
            Create.UniqueConstraint().OnTable("AdminRole").Columns("UserId", "Role");
        }
    }
}