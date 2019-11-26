using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.Migrations.KfDb
{
    public partial class add_conversation_column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HeadImgUrl",
                table: "CustomerServiceConversations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "CustomerServiceConversations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeadImgUrl",
                table: "CustomerServiceConversations");

            migrationBuilder.DropColumn(
                name: "NickName",
                table: "CustomerServiceConversations");
        }
    }
}
