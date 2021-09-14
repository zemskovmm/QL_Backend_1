using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.CourseCatalog.School
{
    [MigrationDate(2021, 8, 23, 11, 00)]
    public class CreateSchoolGallery : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("SchoolGalleries")
                .WithColumn("SchoolId").AsInt32().ForeignKey("Schools", "Id").PrimaryKey()
                .WithColumn("ImageId").AsInt32().ForeignKey("Blobs", "Id").PrimaryKey();
        }
    }
}
