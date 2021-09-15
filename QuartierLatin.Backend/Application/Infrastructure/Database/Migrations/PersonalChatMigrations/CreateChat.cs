using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.PersonalChatMigrations
{
    [MigrationDate(2021, 9, 14, 11, 00)]
    public class CreateChat : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("ChatMessages")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("PortalUserId").AsInt32().ForeignKey("PortalUsers", "Id");
        }
    }
}
