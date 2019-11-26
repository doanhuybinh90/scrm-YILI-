using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.Migrations.KfDb
{
    public partial class add_kf_columns_2018_4_24 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CustomerServiceManager",
                table: "CustomerServiceOnlines",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ConversationScore",
                table: "CustomerServiceConversations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerServiceManager",
                table: "CustomerServiceOnlines");

            migrationBuilder.DropColumn(
                name: "ConversationScore",
                table: "CustomerServiceConversations");
        }
    }
}
