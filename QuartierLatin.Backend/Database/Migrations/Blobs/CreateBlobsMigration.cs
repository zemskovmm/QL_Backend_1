using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.Blobs
{
    [MigrationDate(2020, 10, 8, 16, 05)]
    public class MigrateCreateBlobs : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Blobs")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("FileType").AsString()
                .WithColumn("OriginalFileName").AsString();
        }
    }
}