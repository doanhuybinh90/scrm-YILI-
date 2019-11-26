using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.Migrations
{
    public partial class add_babytext_columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BabyTextType",
                table: "MpBabyTexts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OneYearMonth",
                table: "MpBabyTexts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OneYearWeek",
                table: "MpBabyTexts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverMonth",
                table: "MpBabyTexts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverYear",
                table: "MpBabyTexts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnbornWeek",
                table: "MpBabyTexts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BabyTextType",
                table: "MpBabyTexts");

            migrationBuilder.DropColumn(
                name: "OneYearMonth",
                table: "MpBabyTexts");

            migrationBuilder.DropColumn(
                name: "OneYearWeek",
                table: "MpBabyTexts");

            migrationBuilder.DropColumn(
                name: "OverMonth",
                table: "MpBabyTexts");

            migrationBuilder.DropColumn(
                name: "OverYear",
                table: "MpBabyTexts");

            migrationBuilder.DropColumn(
                name: "UnbornWeek",
                table: "MpBabyTexts");
        }
    }
}
