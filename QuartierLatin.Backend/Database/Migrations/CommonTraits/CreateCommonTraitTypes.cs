using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.CommonTraits
{
    [MigrationDate(2021, 3, 29, 20, 00)]
    public class CreateCommonTraitTypes: AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("CommonTraitTypes")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Names").AsCustom("jsonb")
                .WithColumn("Identifier").AsString().Nullable();
        }
    }
}
