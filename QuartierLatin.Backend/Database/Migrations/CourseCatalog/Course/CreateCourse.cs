using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.CourseCatalog.Course
{
    [MigrationDate(2021, 6, 1, 22, 10)]
    public class CreateCourse : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Courses")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("SchoolId").AsInt32().ForeignKey("Schools", "Id");

            Create.Table("CourseLanguages")
                .WithColumn("CourseId").AsInt32().ForeignKey("Courses", "Id").PrimaryKey()
                .WithColumn("LanguageId").AsInt32().ForeignKey("Languages", "Id").PrimaryKey()
                .WithColumn("Name").AsString()
                .WithColumn("Description").AsString()
                .WithColumn("Url").AsString();
        }
    }
}