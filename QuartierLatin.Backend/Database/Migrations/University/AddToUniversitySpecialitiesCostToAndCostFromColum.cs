using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.University
{
    [MigrationDate(2021, 4, 21, 13, 24)]
    public class AddToUniversitySpecialitiesCostToAndCostFromColum : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("UniversitySpecialties")
                .AddColumn("CostFrom").AsInt32()
                .AddColumn("CostTo").AsInt32();

            Delete.Column("Cost").FromTable("UniversitySpecialties");
        }
    }
}
