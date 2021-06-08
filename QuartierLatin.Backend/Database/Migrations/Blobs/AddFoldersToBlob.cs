using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.Blobs
{
    [MigrationDate(2021, 6, 1, 16, 10)]
    public class AddFoldersToBlob : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("Blobs")
                .AddColumn("StorageFolderId").AsInt32().ForeignKey("StorageFolders", "Id").Nullable();
        }
    }
}
