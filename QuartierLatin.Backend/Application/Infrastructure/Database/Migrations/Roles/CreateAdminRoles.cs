using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.Roles
{
    [MigrationDate(2020, 10, 22, 12, 48)]
    public class CreateAdminRoles : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("AdminRole")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AdminId").AsInt32().NotNullable()
                .WithColumn("Role").AsString().NotNullable();

            Create.ForeignKey().FromTable("AdminRole").ForeignColumn("AdminId").ToTable("Admins").PrimaryColumn("Id");
            Create.UniqueConstraint().OnTable("AdminRole").Columns("AdminId", "Role");
        }
    }
}