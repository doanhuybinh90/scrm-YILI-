using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.Migrations.KfDb
{
    public partial class addconnectstate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MediaId",
                table: "CustomerServiceResponseTexts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviewImgUrl",
                table: "CustomerServiceResponseTexts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReponseContentType",
                table: "CustomerServiceResponseTexts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UseCount",
                table: "CustomerServiceResponseTexts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConnectId",
                table: "CustomerServiceOnlines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConnectState",
                table: "CustomerServiceOnlines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CustomerServicePrivateResponseTexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    MediaId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MpID = table.Column<int>(type: "int", nullable: false),
                    OpenID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviewImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReponseContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UseCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerServicePrivateResponseTexts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerServicePrivateResponseTexts");

            migrationBuilder.DropColumn(
                name: "MediaId",
                table: "CustomerServiceResponseTexts");

            migrationBuilder.DropColumn(
                name: "PreviewImgUrl",
                table: "CustomerServiceResponseTexts");

            migrationBuilder.DropColumn(
                name: "ReponseContentType",
                table: "CustomerServiceResponseTexts");

            migrationBuilder.DropColumn(
                name: "UseCount",
                table: "CustomerServiceResponseTexts");

            migrationBuilder.DropColumn(
                name: "ConnectId",
                table: "CustomerServiceOnlines");

            migrationBuilder.DropColumn(
                name: "ConnectState",
                table: "CustomerServiceOnlines");
        }
    }
}
