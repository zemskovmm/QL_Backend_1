using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.Page
{
    [MigrationDate(2021, 6, 27, 22, 20)]
    public class AddToPageDate : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("Pages")
                .AddColumn("Date").AsDateTime().Nullable();
        }
    }
}
