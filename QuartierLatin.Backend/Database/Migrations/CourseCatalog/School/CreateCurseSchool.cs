using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.CourseCatalog.School
{
    [MigrationDate(2021, 6, 1, 21, 10)]
    public class CreateCourseSchool : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Schools")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("FoundationYear").AsInt32().Nullable();

            Create.Table("SchoolLanguages")
                .WithColumn("SchoolId").AsInt32().ForeignKey("Schools", "Id").PrimaryKey()
                .WithColumn("LanguageId").AsInt32().ForeignKey("Languages", "Id").PrimaryKey()
                .WithColumn("Name").AsString()
                .WithColumn("Description").AsString()
                .WithColumn("Url").AsString();
        }
    }
}