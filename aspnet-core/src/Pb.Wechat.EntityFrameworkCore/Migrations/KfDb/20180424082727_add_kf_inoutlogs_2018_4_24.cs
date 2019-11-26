using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.Migrations.KfDb
{
    public partial class add_kf_inoutlogs_2018_4_24 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArticleDescription",
                table: "CustomerServiceResponseTexts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticleTitle",
                table: "CustomerServiceResponseTexts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticleUrl",
                table: "CustomerServiceResponseTexts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerInOutLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    InOutState = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    MpID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInOutLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArticleDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ArticleGroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ArticleTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ArticleUrl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    ImageName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    MediaId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MpID = table.Column<int>(type: "int", nullable: false),
                    PreviewUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReplyType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VideoDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VideoTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VoiceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerMaterials", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerInOutLogs");

            migrationBuilder.DropTable(
                name: "CustomerMaterials");

            migrationBuilder.DropColumn(
                name: "ArticleDescription",
                table: "CustomerServiceResponseTexts");

            migrationBuilder.DropColumn(
                name: "ArticleTitle",
                table: "CustomerServiceResponseTexts");

            migrationBuilder.DropColumn(
                name: "ArticleUrl",
                table: "CustomerServiceResponseTexts");
        }
    }
}
