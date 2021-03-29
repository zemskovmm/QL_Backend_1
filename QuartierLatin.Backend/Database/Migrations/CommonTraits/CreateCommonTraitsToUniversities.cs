using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.CommonTraits
{
    [MigrationDate(2021, 3, 29, 20, 20)]
    public class CreateCommonTraitsToUniversities : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("CommonTraitsToUniversities")
                .WithColumn("UniversityId").AsInt64().ForeignKey("Universities", "Id").PrimaryKey()
                .WithColumn("CommonTraitId").AsInt64().ForeignKey("CommonTraits", "Id").PrimaryKey();
        }
    }
}
