using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.Blobs
{
    [MigrationDate(2021, 5, 18, 13, 10)]
    public class AddIsDeleteFlagToBlob : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("Blobs")
                .AddColumn("IsDeleted").AsBoolean().WithDefaultValue(false);
        }
    }
}