using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.University
{
    [MigrationDate(2021, 4, 11, 10, 43)]
    public class CreateUniversityInstructionLanguages : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("UniversityInstructionLanguages")
                .WithColumn("UniversityId").AsInt32().ForeignKey("Universities", "Id").PrimaryKey()
                .WithColumn("LanguageId").AsInt32().ForeignKey("Languages", "Id").PrimaryKey();
        }
    }

}