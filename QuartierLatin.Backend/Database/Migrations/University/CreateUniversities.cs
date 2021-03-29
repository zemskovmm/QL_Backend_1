using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.University
{
    [MigrationDate(2021, 3, 29, 19, 50)]
    public class CreateUniversities : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Universities")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity();
        }
    }
}
