using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Pb.Wechat.Migrations
{
    public partial class updatefans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FansState",
                table: "MpFans");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "MpFans");

            migrationBuilder.AlterColumn<bool>(
                name: "IsFans",
                table: "MpFans",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstSubscribeTime",
                table: "MpFans",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstSubscribeTime",
                table: "MpFans");

            migrationBuilder.AlterColumn<string>(
                name: "IsFans",
                table: "MpFans",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "FansState",
                table: "MpFans",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "MpFans",
                nullable: true);
        }
    }
}
