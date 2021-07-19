using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.CommonTraits
{
    [MigrationDate(2021, 6, 7, 20, 10)]
    public class CreateCommonTraitsToSchool : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("CommonTraitsToSchool")
                .WithColumn("SchoolId").AsInt32().ForeignKey("Schools", "Id").PrimaryKey()
                .WithColumn("CommonTraitId").AsInt32().ForeignKey("CommonTraits", "Id").PrimaryKey();
        }
    }
}