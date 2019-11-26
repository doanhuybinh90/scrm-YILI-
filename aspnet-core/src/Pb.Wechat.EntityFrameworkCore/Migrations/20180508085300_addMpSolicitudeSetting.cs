using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.Migrations
{
    public partial class addMpSolicitudeSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MpSolicitudeSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArticleGroupID = table.Column<int>(type: "int", nullable: true),
                    ArticleGroupMediaID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArticleGroupName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArticleID = table.Column<int>(type: "int", nullable: true),
                    ArticleMediaID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArticleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DelayMinutes = table.Column<int>(type: "int", nullable: false),
                    ImageID = table.Column<int>(type: "int", nullable: true),
                    ImageMediaID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MpID = table.Column<int>(type: "int", nullable: false),
                    VideoID = table.Column<int>(type: "int", nullable: true),
                    VideoMediaID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoiceID = table.Column<int>(type: "int", nullable: true),
                    VoiceMediaID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoiceName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MpSolicitudeSettings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MpSolicitudeSettings");
        }
    }
}
