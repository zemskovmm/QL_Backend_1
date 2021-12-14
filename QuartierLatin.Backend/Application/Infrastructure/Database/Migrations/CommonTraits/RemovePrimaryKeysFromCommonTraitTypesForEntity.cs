using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.CommonTraits
{
    [MigrationDate(2021, 12, 14, 01, 30)]
    public class RemovePrimaryKeysFromCommonTraitTypesForEntity : AutoReversingMigration
    {
        public override void Up()
        {
			
			Delete.PrimaryKey("PK_CommonTraitTypesForEntites").FromTable("CommonTraitTypesForEntites");


        }
    }
}
