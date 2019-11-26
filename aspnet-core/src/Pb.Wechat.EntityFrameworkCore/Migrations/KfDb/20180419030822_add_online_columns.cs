using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.Migrations.KfDb
{
    public partial class add_online_columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AutoJoinReply",
                table: "CustomerServiceOnlines",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AutoJoinReplyText",
                table: "CustomerServiceOnlines",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AutoLeaveReply",
                table: "CustomerServiceOnlines",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AutoLeaveReplyText",
                table: "CustomerServiceOnlines",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoJoinReply",
                table: "CustomerServiceOnlines");

            migrationBuilder.DropColumn(
                name: "AutoJoinReplyText",
                table: "CustomerServiceOnlines");

            migrationBuilder.DropColumn(
                name: "AutoLeaveReply",
                table: "CustomerServiceOnlines");

            migrationBuilder.DropColumn(
                name: "AutoLeaveReplyText",
                table: "CustomerServiceOnlines");
        }
    }
}
