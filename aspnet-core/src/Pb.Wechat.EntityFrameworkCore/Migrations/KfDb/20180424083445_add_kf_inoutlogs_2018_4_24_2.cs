using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.Migrations.KfDb
{
    public partial class add_kf_inoutlogs_2018_4_24_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "CustomerInOutLogs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CustomerInOutLogs");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "CustomerInOutLogs");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "CustomerInOutLogs");

            migrationBuilder.DropColumn(
                name: "MpID",
                table: "CustomerInOutLogs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "CustomerInOutLogs",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CustomerInOutLogs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "CustomerInOutLogs",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "CustomerInOutLogs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MpID",
                table: "CustomerInOutLogs",
                nullable: false,
                defaultValue: 0);
        }
    }
}
