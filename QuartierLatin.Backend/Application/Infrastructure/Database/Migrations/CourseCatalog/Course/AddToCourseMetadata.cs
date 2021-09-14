using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.CourseCatalog.Course
{
    [MigrationDate(2021, 7, 8, 16, 30)]
    public class AddToCourseMetadata : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("CourseLanguages")
                .AddColumn("Metadata").AsCustom("jsonb").Nullable();
        }
    }
}
