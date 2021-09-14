using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.University
{
    [MigrationDate(2021, 3, 29, 19, 55)]
    public class CreateUniversityLanguages : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("UniversityLanguages")
                .WithColumn("UniversityId").AsInt32().ForeignKey("Universities", "Id").PrimaryKey()
                .WithColumn("LanguageId").AsInt32().ForeignKey("Languages", "Id").PrimaryKey()
                .WithColumn("Name").AsString()
                .WithColumn("Description").AsString()
                .WithColumn("Url").AsString();
        }
    }
}
