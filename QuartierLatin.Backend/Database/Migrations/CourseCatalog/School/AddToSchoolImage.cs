using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.CourseCatalog.School
{
    [MigrationDate(2021, 8, 13, 16, 40)]
    public class AddToSchoolImage : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("Schools")
                .AddColumn("ImageId").AsInt32().ForeignKey("Blobs", "Id").Nullable();
        }
    }
}
