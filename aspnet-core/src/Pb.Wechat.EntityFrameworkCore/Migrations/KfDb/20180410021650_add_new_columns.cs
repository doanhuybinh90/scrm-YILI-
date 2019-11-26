using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.Migrations.KfDb
{
    public partial class add_new_columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ReponseContentType",
                table: "CustomerServiceResponseTexts",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CustomerServicePrivateResponseTexts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "CustomerServicePrivateResponseTexts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KfType",
                table: "CustomerServiceOnlines",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "CustomerServicePrivateResponseTexts");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "CustomerServicePrivateResponseTexts");

            migrationBuilder.DropColumn(
                name: "KfType",
                table: "CustomerServiceOnlines");

            migrationBuilder.AlterColumn<string>(
                name: "ReponseContentType",
                table: "CustomerServiceResponseTexts",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
