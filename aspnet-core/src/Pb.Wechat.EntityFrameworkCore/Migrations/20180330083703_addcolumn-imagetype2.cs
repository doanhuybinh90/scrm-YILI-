using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.Migrations
{
    public partial class addcolumnimagetype2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MpMediaImageType",
                table: "MpMediaImages");

            migrationBuilder.AddColumn<int>(
                name: "MediaImageType",
                table: "MpMediaImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MediaImageTypeName",
                table: "MpMediaImages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaImageType",
                table: "MpMediaImages");

            migrationBuilder.DropColumn(
                name: "MediaImageTypeName",
                table: "MpMediaImages");

            migrationBuilder.AddColumn<int>(
                name: "MpMediaImageType",
                table: "MpMediaImages",
                nullable: false,
                defaultValue: 0);
        }
    }
}
