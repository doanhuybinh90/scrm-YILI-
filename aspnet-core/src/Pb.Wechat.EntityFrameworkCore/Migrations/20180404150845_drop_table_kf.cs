using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.Migrations
{
    public partial class drop_table_kf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerServiceOnlines");

            migrationBuilder.DropTable(
                name: "CustomerServiceResponseTexts");

            migrationBuilder.DropTable(
                name: "CustomerServiceWorkTimes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerServiceOnlines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    InviteExpireTime = table.Column<string>(nullable: true),
                    InviteStatus = table.Column<string>(nullable: true),
                    InviteWx = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    KfAccount = table.Column<string>(nullable: true),
                    KfHeadingUrl = table.Column<string>(nullable: true),
                    KfId = table.Column<string>(nullable: true),
                    KfNick = table.Column<string>(nullable: true),
                    KfPassWord = table.Column<string>(nullable: true),
                    KfWx = table.Column<string>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    LocalHeadFilePath = table.Column<string>(nullable: true),
                    LocalHeadingUrl = table.Column<string>(nullable: true),
                    MpID = table.Column<int>(nullable: false),
                    PreKfAccount = table.Column<string>(nullable: true),
                    PublicNumberAccount = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerServiceOnlines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerServiceResponseTexts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    MpID = table.Column<int>(nullable: false),
                    ResponseText = table.Column<string>(nullable: true),
                    ResponseType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerServiceResponseTexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerServiceWorkTimes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AfternoonEndHour = table.Column<string>(nullable: true),
                    AfternoonEndMinute = table.Column<string>(nullable: true),
                    AfternoonStartHour = table.Column<string>(nullable: true),
                    AfternoonStartMinute = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    MorningEndHour = table.Column<string>(nullable: true),
                    MorningEndMinute = table.Column<string>(nullable: true),
                    MorningStartHour = table.Column<string>(nullable: true),
                    MorningStartMinute = table.Column<string>(nullable: true),
                    MpID = table.Column<int>(nullable: false),
                    WeekDay = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerServiceWorkTimes", x => x.Id);
                });
        }
    }
}
