using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Pb.Wechat.Migrations
{
    public partial class add_tables_memType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BaySex",
                table: "MpMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "BeginBabyBirthday",
                table: "MpMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "BeginPointsBalance",
                table: "MpMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChannelID",
                table: "MpMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ChannelName",
                table: "MpMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndBabyBirthday",
                table: "MpMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EndPointsBalance",
                table: "MpMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IsMember",
                table: "MpMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastBuyProduct",
                table: "MpMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberCategory",
                table: "MpMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotherType",
                table: "MpMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficialCity",
                table: "MpMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizeCity",
                table: "MpMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetID",
                table: "MpMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TargetName",
                table: "MpMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotherType",
                table: "MpGroupItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaySex",
                table: "MpMessages");

            migrationBuilder.DropColumn(
                name: "BeginBabyBirthday",
                table: "MpMessages");

            migrationBuilder.DropColumn(
                name: "BeginPointsBalance",
                table: "MpMessages");

            migrationBuilder.DropColumn(
                name: "ChannelID",
                table: "MpMessages");

            migrationBuilder.DropColumn(
                name: "ChannelName",
                table: "MpMessages");

            migrationBuilder.DropColumn(
                name: "EndBabyBirthday",
                table: "MpMessages");

            migrationBuilder.DropColumn(
                name: "EndPointsBalance",
                table: "MpMessages");

            migrationBuilder.DropColumn(
                name: "IsMember",
                table: "MpMessages");

            migrationBuilder.DropColumn(
                name: "LastBuyProduct",
                table: "MpMessages");

            migrationBuilder.DropColumn(
                name: "MemberCategory",
                table: "MpMessages");

            migrationBuilder.DropColumn(
                name: "MotherType",
                table: "MpMessages");

            migrationBuilder.DropColumn(
                name: "OfficialCity",
                table: "MpMessages");

            migrationBuilder.DropColumn(
                name: "OrganizeCity",
                table: "MpMessages");

            migrationBuilder.DropColumn(
                name: "TargetID",
                table: "MpMessages");

            migrationBuilder.DropColumn(
                name: "TargetName",
                table: "MpMessages");

            migrationBuilder.DropColumn(
                name: "MotherType",
                table: "MpGroupItems");
        }
    }
}
