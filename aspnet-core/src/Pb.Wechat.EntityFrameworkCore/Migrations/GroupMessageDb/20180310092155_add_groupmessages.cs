using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Pb.Wechat.Migrations.GroupMessageDb
{
    public partial class add_groupmessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskGroupMessages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArticleGroupID = table.Column<int>(type: "int", nullable: true),
                    ArticleGroupMediaID = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ArticleGroupName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ArticleID = table.Column<int>(type: "int", nullable: true),
                    ArticleMediaID = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ArticleName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    GroupID = table.Column<int>(type: "int", nullable: false),
                    ImageID = table.Column<int>(type: "int", nullable: true),
                    ImageMediaID = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ImageName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    MessageID = table.Column<int>(type: "int", nullable: false),
                    MessageType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MpID = table.Column<int>(type: "int", nullable: false),
                    VideoID = table.Column<int>(type: "int", nullable: true),
                    VideoMediaID = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VideoName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VoiceID = table.Column<int>(type: "int", nullable: true),
                    VoiceMediaID = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VoiceName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskGroupMessages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskGroupMessages");
        }
    }
}
