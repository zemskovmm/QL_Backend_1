using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.University
{
    [MigrationDate(2021, 4,19, 9,32)]
    public class RemoveUniversityInstructionLanguages : Migration
    {
        public override void Up()
        {
            Delete.Table("UniversityInstructionLanguages");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}