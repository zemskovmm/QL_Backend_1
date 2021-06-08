using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.CurseCatalog.Curse
{
    [MigrationDate(2021, 6, 1, 22, 10)]
    public class CreateCurse : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Curses")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("SchoolId").AsInt32().ForeignKey("Schools", "Id").PrimaryKey();

            Create.Table("CurseLanguages")
                .WithColumn("CurseId").AsInt32().ForeignKey("Curses", "Id").PrimaryKey()
                .WithColumn("LanguageId").AsInt32().ForeignKey("Languages", "Id").PrimaryKey()
                .WithColumn("Name").AsString()
                .WithColumn("Description").AsString()
                .WithColumn("Url").AsString();
        }
    }
}