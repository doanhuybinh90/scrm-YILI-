using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.Migrations
{
    public partial class AddMpSolicitudeText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UseSolicitude",
                table: "MpMenus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MpSolicitudeTexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BabyAge = table.Column<int>(type: "int", nullable: true),
                    BeginDay = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    EndDay = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    MpID = table.Column<int>(type: "int", nullable: false),
                    OneYearMonth = table.Column<int>(type: "int", nullable: false),
                    OneYearWeek = table.Column<int>(type: "int", nullable: false),
                    OverMonth = table.Column<int>(type: "int", nullable: false),
                    OverYear = table.Column<int>(type: "int", nullable: false),
                    SolicitudeText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SolicitudeTextType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UnbornWeek = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MpSolicitudeTexts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MpSolicitudeTexts");

            migrationBuilder.DropColumn(
                name: "UseSolicitude",
                table: "MpMenus");
        }
    }
}
