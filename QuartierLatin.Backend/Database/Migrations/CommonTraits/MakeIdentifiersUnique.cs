using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.CommonTraits
{
    [MigrationDate(2021, 4, 19, 9, 59)]
    public class MakeIdentifiersUnique : AutoReversingMigration
    {
        public override void Up()
        {
            Create.UniqueConstraint().OnTable("CommonTraitTypes").Column("Identifier");
            Create.UniqueConstraint().OnTable("CommonTraits").Columns("CommonTraitTypeId", "Identifier");
        }
    }
}