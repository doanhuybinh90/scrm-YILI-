using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.Migrations.KfDb
{
    public partial class create_db_kf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerServiceOnlines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    InviteExpireTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InviteStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InviteWx = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    KfAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KfHeadingUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KfId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KfNick = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KfPassWord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KfWx = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    LocalHeadFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalHeadingUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MpID = table.Column<int>(type: "int", nullable: false),
                    PreKfAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicNumberAccount = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerServiceOnlines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerServiceResponseTexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    MpID = table.Column<int>(type: "int", nullable: false),
                    ResponseText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerServiceResponseTexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerServiceWorkTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AfternoonEndHour = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AfternoonEndMinute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AfternoonStartHour = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AfternoonStartMinute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    MorningEndHour = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MorningEndMinute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MorningStartHour = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MorningStartMinute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MpID = table.Column<int>(type: "int", nullable: false),
                    WeekDay = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerServiceWorkTimes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerServiceOnlines");

            migrationBuilder.DropTable(
                name: "CustomerServiceResponseTexts");

            migrationBuilder.DropTable(
                name: "CustomerServiceWorkTimes");
        }
    }
}
