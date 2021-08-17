using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.CourseCatalog.Course
{
    [MigrationDate(2021, 8, 17, 12, 16)]
    public class AddToCoursePrice : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("Courses")
                .AddColumn("Price").AsInt32().WithDefaultValue(0);
        }
    }
}
