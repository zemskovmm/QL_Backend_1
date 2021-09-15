using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.PersonalChatMigrations
{
    [MigrationDate(2021, 9, 14, 11, 20)]
    public class CreateChatMessages : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("ChatMessages")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("MessageType").AsInt16()
                .WithColumn("Author").AsString()
                .WithColumn("Text").AsString()
                .WithColumn("BlobId").AsInt32().ForeignKey("Blobs", "Id")
                .WithColumn("ChatId").AsInt32().ForeignKey("Chats", "Id");
        }
    }
}
