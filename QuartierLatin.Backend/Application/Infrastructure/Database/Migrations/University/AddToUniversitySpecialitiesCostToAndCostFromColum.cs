using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.University
{
    [MigrationDate(2021, 4, 21, 13, 24)]
    public class AddToUniversitySpecialitiesCostToAndCostFromColum : Migration
    {
        public override void Up()
        {
            Rename.Column("Cost").OnTable("UniversitySpecialties").To("CostFrom");

            Alter.Table("UniversitySpecialties")
                .AddColumn("CostTo").AsInt32().Nullable();

            Execute.Sql("UPDATE \"UniversitySpecialties\" SET \"CostTo\" = \"CostFrom\"");
            Alter.Column("CostTo").OnTable("UniversitySpecialties").AsInt32().NotNullable();

        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}
