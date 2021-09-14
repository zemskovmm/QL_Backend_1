using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.CommonTraits
{
    [MigrationDate(2021, 5, 31, 16, 10)]
    public class AddOrderToCommonTraitType : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("CommonTraitTypes")
                .AddColumn("Order").AsInt32().WithDefaultValue(0);
        }
    }
}
