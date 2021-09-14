using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.Page
{
    [MigrationDate(2021, 7, 8, 16, 10)]
    public class AddToPageMetadata : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("Pages")
                .AddColumn("Metadata").AsCustom("jsonb").Nullable();
        }
    }
}
