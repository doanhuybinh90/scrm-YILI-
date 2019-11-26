using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.Migrations
{
    public partial class add_babytext_begindayendday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BeginDay",
                table: "MpBabyTexts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EndDay",
                table: "MpBabyTexts",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeginDay",
                table: "MpBabyTexts");

            migrationBuilder.DropColumn(
                name: "EndDay",
                table: "MpBabyTexts");
        }
    }
}
