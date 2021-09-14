using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.AppStateEntryMigrations
{
    [MigrationDate(2021, 7, 7, 16, 10)]
    public class CreateAppStateEntry : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("AppStateEntrys")
                .WithColumn("Key").AsString().PrimaryKey()
                .WithColumn("Value").AsString();
        }
    }
}
