using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.University
{
    [MigrationDate(2021, 7, 8, 16, 20)]
    public class AddToUniversityMetadata : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("UniversityLanguages")
                .AddColumn("Metadata").AsCustom("jsonb").Nullable();
        }
    }
}
