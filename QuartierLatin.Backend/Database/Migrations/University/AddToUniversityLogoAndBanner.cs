using FluentMigrator;
using System;

namespace QuartierLatin.Backend.Database.Migrations.University
{
    [MigrationDate(2021, 6, 17, 11, 10)]
    public class AddToUniversityLogoAndBanner : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("Universities")
                .AddColumn("LogoId").AsInt32().ForeignKey("Blobs", "Id").Nullable()
                .AddColumn("BannerId").AsInt32().ForeignKey("Blobs", "Id").Nullable();
        }
    }
}
