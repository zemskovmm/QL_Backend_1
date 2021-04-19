using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.University
{
    [MigrationDate(2021, 4, 11, 13, 24)]
    public class CreateSpecialties : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("SpecialtyCategories")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Names").AsCustom("jsonb");

            Create.Table("Specialties")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("CategoryId").AsInt32().ForeignKey("SpecialtyCategories", "Id")
                .WithColumn("Names").AsCustom("jsonb");

            Create.Table("UniversitySpecialties")
                .WithColumn("UniversityId").AsInt32().PrimaryKey().ForeignKey("Universities", "Id")
                .WithColumn("SpecialtyId").AsInt32().PrimaryKey().ForeignKey("Specialties", "Id")
                .WithColumn("Cost").AsInt32();
        }
    }
}