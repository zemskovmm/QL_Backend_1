using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.CourseCatalog.Course
{
    [MigrationDate(2021, 8, 23, 11, 15)]
    public class CreateCourseGallery : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("CourseGalleries")
                .WithColumn("CourseId").AsInt32().ForeignKey("Courses", "Id").PrimaryKey()
                .WithColumn("ImageId").AsInt32().ForeignKey("Blobs", "Id").PrimaryKey();
        }
    }
}
