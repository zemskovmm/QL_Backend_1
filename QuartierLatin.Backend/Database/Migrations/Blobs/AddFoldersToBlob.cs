using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.Blobs
{
    public class AddFoldersToBlob : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("Blobs")
                .AddColumn("StorageFolderId").AsInt32().ForeignKey("StorageFolders", "Id").Nullable();
        }
    }
}
