using Microsoft.EntityFrameworkCore.Migrations;

namespace Pb.Wechat.Migrations.GroupMessageDb
{
    public partial class add_table_msg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticleGroupID",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "ArticleGroupMediaID",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "ArticleGroupName",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "ArticleID",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "ArticleMediaID",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "ArticleName",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "ImageID",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "ImageMediaID",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "MessageType",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "VideoID",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "VideoMediaID",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "VideoName",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "VoiceID",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "VoiceMediaID",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "VoiceName",
                table: "TaskGroupMessages");

            migrationBuilder.AddColumn<string>(
                name: "ClearedOpenIDs",
                table: "TaskGroupMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrCode",
                table: "TaskGroupMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrMsg",
                table: "TaskGroupMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FailCount",
                table: "TaskGroupMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MsgDataID",
                table: "TaskGroupMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OpenIDs",
                table: "TaskGroupMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SendCount",
                table: "TaskGroupMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SuccessCount",
                table: "TaskGroupMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TaskID",
                table: "TaskGroupMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskState",
                table: "TaskGroupMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WxMsgID",
                table: "TaskGroupMessages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClearedOpenIDs",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "ErrCode",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "ErrMsg",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "FailCount",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "MsgDataID",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "OpenIDs",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "SendCount",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "SuccessCount",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "TaskID",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "TaskState",
                table: "TaskGroupMessages");

            migrationBuilder.DropColumn(
                name: "WxMsgID",
                table: "TaskGroupMessages");

            migrationBuilder.AddColumn<int>(
                name: "ArticleGroupID",
                table: "TaskGroupMessages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticleGroupMediaID",
                table: "TaskGroupMessages",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticleGroupName",
                table: "TaskGroupMessages",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ArticleID",
                table: "TaskGroupMessages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticleMediaID",
                table: "TaskGroupMessages",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticleName",
                table: "TaskGroupMessages",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "TaskGroupMessages",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageID",
                table: "TaskGroupMessages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageMediaID",
                table: "TaskGroupMessages",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "TaskGroupMessages",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MessageType",
                table: "TaskGroupMessages",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VideoID",
                table: "TaskGroupMessages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoMediaID",
                table: "TaskGroupMessages",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoName",
                table: "TaskGroupMessages",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VoiceID",
                table: "TaskGroupMessages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VoiceMediaID",
                table: "TaskGroupMessages",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VoiceName",
                table: "TaskGroupMessages",
                maxLength: 500,
                nullable: true);
        }
    }
}
