using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.University
{
    [MigrationDate(2021, 4, 27, 11, 2)]
    public class RemoveSpecialtyCosts : Migration
    {
        public override void Up()
        {
            Delete.Column("CostFrom").FromTable("UniversitySpecialties");
            Delete.Column("CostTo").FromTable("UniversitySpecialties");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}