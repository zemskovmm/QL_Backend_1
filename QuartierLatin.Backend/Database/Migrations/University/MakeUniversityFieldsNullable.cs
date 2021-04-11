using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.University
{
    [MigrationDate(2021,4, 10, 22, 35)]
    public class MigrateMakeUniversityFieldsNullable : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("Universities")
                .AlterColumn("Website").AsString().Nullable()
                .AlterColumn("FoundationYear").AsInt32().Nullable();
        }
    }
}