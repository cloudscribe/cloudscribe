using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.Storage.EF.Migrations
{
    public partial class Limitfieldsize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "cs_User",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "cs_User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "cs_User",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "cs_User",
                nullable: true);
        }
    }
}
