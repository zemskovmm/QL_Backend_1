﻿using FluentMigrator;

namespace QuartierLatin.Backend.Database.Migrations.Page
{
    [MigrationDate(2021, 6, 27, 22, 20)]
    public class AddToPageDate : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("Pages")
                .AlterColumn("Date").AsDateTime().Nullable();
        }
    }
}
