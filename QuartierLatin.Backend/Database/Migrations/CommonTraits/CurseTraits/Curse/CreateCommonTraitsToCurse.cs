using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.CommonTraits.CurseTraits.Curse
{
    [MigrationDate(2021, 6, 8, 20, 15)]
    public class CreateCommonTraitsToCurse : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("CommonTraitToCurse")
                .WithColumn("CurseId").AsInt32().ForeignKey("Curses", "Id").PrimaryKey()
                .WithColumn("CommonTraitId").AsInt32().ForeignKey("CommonTraits", "Id").PrimaryKey();
        }
    }
}
