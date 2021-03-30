using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.University
{
    [MigrationDate(2021, 3, 29, 19, 55)]
    public class CreateUniversityLanguages : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("UniversityLanguages")
                .WithColumn("UniversityId").AsInt64().ForeignKey("Universities", "Id").PrimaryKey()
                .WithColumn("LanguageId").AsInt64().ForeignKey("Languages", "Id").PrimaryKey()
                .WithColumn("Name").AsString()
                .WithColumn("Description").AsString()
                .WithColumn("Url").AsString();
        }
    }
}
