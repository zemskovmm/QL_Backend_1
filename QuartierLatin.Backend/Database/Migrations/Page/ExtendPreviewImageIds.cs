using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.Page
{
    [MigrationDate(2021, 07, 26, 19, 26)]
    public class ExtendPreviewImageIds : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("Pages")
                .AddColumn("SmallPreviewImageId").AsInt64().Nullable()
                .AddColumn("WidePreviewImageId").AsInt64().Nullable();
        }
    }
}