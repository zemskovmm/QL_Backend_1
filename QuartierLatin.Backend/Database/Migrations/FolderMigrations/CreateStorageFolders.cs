using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.FolderMigrations
{
    [MigrationDate(2021, 5, 10, 16, 05)]
    public class CreateStorageFolders : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("StorageFolders")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("FolderName").AsString()
                .WithColumn("FolderParentId").AsInt32().ForeignKey("StorageFolders", "Id").Nullable();
        }
    }
}
