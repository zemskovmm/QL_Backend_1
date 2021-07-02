using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.ImageStandardSize
{
    [MigrationDate(2021, 6, 27, 22, 30)]
    public class CreateImageStandardSize : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("ImageStandardSizes")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Width").AsInt32()
                .WithColumn("Height").AsInt32();
        }
    }
}
