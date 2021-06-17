using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.University
{
    [MigrationDate(2021, 6, 17, 11, 00)]
    public class CreateUniversityGalleries : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("UniversityGalleries")
                .WithColumn("UniversityId").AsInt32().ForeignKey("Universities", "Id").PrimaryKey()
                .WithColumn("ImageId").AsInt32().ForeignKey("Blobs", "Id").PrimaryKey();
        }
    }
}
