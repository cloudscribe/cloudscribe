using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Core.Storage.EF.Migrations
{
    public partial class RemoveCurrecncy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cs_Currency");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cs_Currency",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Code = table.Column<string>(nullable: false),
                    CultureCode = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cs_Currency", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cs_Currency_Code",
                table: "cs_Currency",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_cs_Currency_CultureCode",
                table: "cs_Currency",
                column: "CultureCode");
        }
    }
}
