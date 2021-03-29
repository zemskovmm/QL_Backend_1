using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.CommonTraits
{
    [MigrationDate(2021, 3, 29, 20, 10)]
    public class CreateCommonTraits : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("CommonTraits")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("CommonTraitTypeId").AsInt64().ForeignKey("CommonTraitTypes", "Id")
                .WithColumn("Names").AsCustom("jsonb")
                .WithColumn("IconBlobId").AsInt64().ForeignKey("Blobs", "Id")
                .WithColumn("Order").AsInt64();
        }
    }
}
