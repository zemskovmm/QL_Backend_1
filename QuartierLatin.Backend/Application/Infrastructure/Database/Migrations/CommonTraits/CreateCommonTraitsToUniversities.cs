using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.CommonTraits
{
    [MigrationDate(2021, 3, 29, 20, 20)]
    public class CreateCommonTraitsToUniversities : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("CommonTraitsToUniversities")
                .WithColumn("UniversityId").AsInt32().ForeignKey("Universities", "Id").PrimaryKey()
                .WithColumn("CommonTraitId").AsInt32().ForeignKey("CommonTraits", "Id").PrimaryKey();
        }
    }
}
