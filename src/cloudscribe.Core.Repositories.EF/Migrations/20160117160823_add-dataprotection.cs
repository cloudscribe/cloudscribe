using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace cloudscribe.Core.Repositories.EF.Migrations
{
    public partial class adddataprotection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "mp_Sites",
                type: "datetime",
                nullable: false,
                defaultValueSql: "getutcdate()");
            migrationBuilder.AddColumn<bool>(
                name: "IsDataProtected",
                table: "mp_Sites",
                type: "bit",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "CreatedUtc", table: "mp_Sites");
            migrationBuilder.DropColumn(name: "IsDataProtected", table: "mp_Sites");
        }
    }
}
