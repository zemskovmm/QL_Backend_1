using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.PersonalChatMigrations
{
    [MigrationDate(2021, 9, 14, 9, 50)]
    public class CreateChat : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Chats")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("PortalUserId").AsInt32().ForeignKey("PortalUsers", "Id")
                .WithColumn("ApplicationId").AsInt32().ForeignKey("PortalApplications", "Id");
        }
    }
}
