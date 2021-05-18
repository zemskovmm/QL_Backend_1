using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.FolderMigrations
{
    [MigrationDate(2021, 5, 17, 12, 10)]
    public class AddStorageFolderIsDeleteFlag : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("StorageFolders")
                .AddColumn("IsDeleted").AsBoolean().WithDefaultValue(false);
        }
    }
}
