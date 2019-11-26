using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.Migrations.KfDb
{
    public partial class add_column_onlineservice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AutoJoin",
                table: "CustomerServiceOnlines",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AutoJoinCount",
                table: "CustomerServiceOnlines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OpenID",
                table: "CustomerServiceOnlines",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoJoin",
                table: "CustomerServiceOnlines");

            migrationBuilder.DropColumn(
                name: "AutoJoinCount",
                table: "CustomerServiceOnlines");

            migrationBuilder.DropColumn(
                name: "OpenID",
                table: "CustomerServiceOnlines");
        }
    }
}
