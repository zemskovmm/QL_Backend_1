using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.CourseCatalog.Course
{
    [MigrationDate(2021, 8, 13, 16, 30)]
    public class AddToCourseImage : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("Courses")
                .AddColumn("ImageId").AsInt32().ForeignKey("Blobs", "Id").Nullable();
        }
    }
}
