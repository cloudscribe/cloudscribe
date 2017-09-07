using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace cloudscribe.Core.IdentityServer.EFCore.MySql.Migrations
{
    public partial class changes20170820 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoutSessionRequired",
                table: "csids_Clients");

            migrationBuilder.DropColumn(
                name: "LogoutUri",
                table: "csids_Clients");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_IdentityResources",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_IdentityClaims",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ClientSecrets",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ClientScopes",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_Clients",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<bool>(
                name: "AlwaysIncludeUserClaimsInIdToken",
                table: "csids_Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BackChannelLogoutSessionRequired",
                table: "csids_Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BackChannelLogoutUri",
                table: "csids_Clients",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FrontChannelLogoutSessionRequired",
                table: "csids_Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FrontChannelLogoutUri",
                table: "csids_Clients",
                type: "longtext",
                nullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ClientRedirectUris",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ClientPostLogoutRedirectUris",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ClientIdPRestrictions",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ClientGrantTypes",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ClientCorsOrigins",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ClientClaims",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ApiSecrets",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ApiScopes",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ApiScopeClaims",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ApiResources",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ApiClaims",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlwaysIncludeUserClaimsInIdToken",
                table: "csids_Clients");

            migrationBuilder.DropColumn(
                name: "BackChannelLogoutSessionRequired",
                table: "csids_Clients");

            migrationBuilder.DropColumn(
                name: "BackChannelLogoutUri",
                table: "csids_Clients");

            migrationBuilder.DropColumn(
                name: "FrontChannelLogoutSessionRequired",
                table: "csids_Clients");

            migrationBuilder.DropColumn(
                name: "FrontChannelLogoutUri",
                table: "csids_Clients");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_IdentityResources",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_IdentityClaims",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ClientSecrets",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ClientScopes",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_Clients",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<bool>(
                name: "LogoutSessionRequired",
                table: "csids_Clients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LogoutUri",
                table: "csids_Clients",
                nullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ClientRedirectUris",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ClientPostLogoutRedirectUris",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ClientIdPRestrictions",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ClientGrantTypes",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ClientCorsOrigins",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ClientClaims",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ApiSecrets",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ApiScopes",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ApiScopeClaims",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ApiResources",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "csids_ApiClaims",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);
        }
    }
}
