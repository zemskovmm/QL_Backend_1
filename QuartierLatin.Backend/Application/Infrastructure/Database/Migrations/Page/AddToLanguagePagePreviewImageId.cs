using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.Page
{
    [MigrationDate(2021, 6, 1, 23, 10)]
    public class AddToLanguagePagePreviewImageId : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("Pages")
                .AddColumn("PreviewImageId").AsInt32().ForeignKey("Blobs", "Id").Nullable();
        }
    }
}