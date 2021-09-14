using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.CourseCatalog.School
{
    [MigrationDate(2021, 7, 8, 16, 40)]
    public class AddToSchoolMetadata : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("SchoolLanguages")
                .AddColumn("Metadata").AsCustom("jsonb").Nullable();
        }
    }
}
