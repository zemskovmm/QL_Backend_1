using FluentMigrator;

namespace QuartierLatin.Backend.Application.Infrastructure.Database.Migrations.University
{
    [MigrationDate(2021, 4, 26, 23, 49)]
    public class CreateDegrees : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Degrees")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Names").AsCustom("jsonb");

            Create.Table("UniversityDegrees")
                .WithColumn("UniversityId").AsInt32().PrimaryKey()
                .ForeignKey("Universities", "Id")
                .WithColumn("DegreeId").AsInt32().PrimaryKey()
                .ForeignKey("Degrees", "Id")
                .WithColumn("CostGroup").AsInt32();
        }
    }
}