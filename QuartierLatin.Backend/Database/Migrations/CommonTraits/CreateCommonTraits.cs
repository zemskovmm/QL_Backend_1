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
                .WithColumn("CommonTraitTypeId").AsInt32().ForeignKey("CommonTraitTypes", "Id")
                .WithColumn("Names").AsCustom("jsonb")
                .WithColumn("IconBlobId").AsInt32().ForeignKey("Blobs", "Id").Nullable()
                .WithColumn("Order").AsInt32()
                .WithColumn("Identifier").AsString().Nullable()
                .WithColumn("ParentId").AsInt32().ForeignKey("CommonTraits", "Id").Nullable();
        }
    }
}
