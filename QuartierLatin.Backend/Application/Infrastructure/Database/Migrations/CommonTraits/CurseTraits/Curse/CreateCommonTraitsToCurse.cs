using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.CommonTraits.CurseTraits.Curse
{
    [MigrationDate(2021, 6, 8, 20, 15)]
    public class CreateCommonTraitsToCourse : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("CommonTraitToCourse")
                .WithColumn("CourseId").AsInt32().ForeignKey("Courses", "Id").PrimaryKey()
                .WithColumn("CommonTraitId").AsInt32().ForeignKey("CommonTraits", "Id").PrimaryKey();
        }
    }
}
