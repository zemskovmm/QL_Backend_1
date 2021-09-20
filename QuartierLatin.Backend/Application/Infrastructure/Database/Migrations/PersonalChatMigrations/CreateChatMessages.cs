using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.PersonalChatMigrations
{
    [MigrationDate(2021, 9, 14, 10, 55)]
    public class CreateChatMessages : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("ChatMessages")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("MessageType").AsInt16()
                .WithColumn("Author").AsString()
                .WithColumn("Text").AsString().Nullable()
                .WithColumn("BlobId").AsInt32().ForeignKey("Blobs", "Id").Nullable()
                .WithColumn("ChatId").AsInt32().ForeignKey("Chats", "Id")
                .WithColumn("Date").AsDateTime();
        }
    }
}
