using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.Page
{
    [MigrationDate(2021, 3, 14, 23, 20)]
    public class CreateDataBlocks : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("DataBlocks")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Type").AsString().NotNullable()
                .WithColumn("BlockData").AsString().NotNullable()
                .WithColumn("LanguageId").AsInt64().NotNullable()
                .WithColumn("PageId").AsInt64().NotNullable()
                .WithColumn("BlockRootId").AsInt64().NotNullable();
        }
    }
}
