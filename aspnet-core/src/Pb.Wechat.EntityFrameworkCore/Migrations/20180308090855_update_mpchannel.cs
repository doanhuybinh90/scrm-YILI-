using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.Migrations
{
    public partial class update_mpchannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkUrl",
                table: "MpChannels");

            migrationBuilder.AlterColumn<string>(
                name: "PushActivityName",
                table: "MpChannels",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500);

            migrationBuilder.AddColumn<int>(
                name: "ArticleGroupID",
                table: "MpChannels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticleGroupMediaID",
                table: "MpChannels",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticleGroupName",
                table: "MpChannels",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ArticleID",
                table: "MpChannels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticleMediaID",
                table: "MpChannels",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticleName",
                table: "MpChannels",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChannelType",
                table: "MpChannels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "MpChannels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "MpChannels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EventKey",
                table: "MpChannels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "MpChannels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "MpChannels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageID",
                table: "MpChannels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageMediaID",
                table: "MpChannels",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "MpChannels",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediaID",
                table: "MpChannels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReplyType",
                table: "MpChannels",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VideoID",
                table: "MpChannels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoMediaID",
                table: "MpChannels",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoName",
                table: "MpChannels",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VoiceID",
                table: "MpChannels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VoiceMediaID",
                table: "MpChannels",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VoiceName",
                table: "MpChannels",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticleGroupID",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "ArticleGroupMediaID",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "ArticleGroupName",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "ArticleID",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "ArticleMediaID",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "ArticleName",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "ChannelType",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "EventKey",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "ImageID",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "ImageMediaID",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "MediaID",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "ReplyType",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "VideoID",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "VideoMediaID",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "VideoName",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "VoiceID",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "VoiceMediaID",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "VoiceName",
                table: "MpChannels");

            migrationBuilder.AlterColumn<string>(
                name: "PushActivityName",
                table: "MpChannels",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkUrl",
                table: "MpChannels",
                nullable: true);
        }
    }
}
