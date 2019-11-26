using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pb.Wechat.Migrations.KfDb
{
    public partial class add_newtables_20184251 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "CustomerMediaVoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TypeName",
                table: "CustomerMediaVoices",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "CustomerMediaVideos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TypeName",
                table: "CustomerMediaVideos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "CustomerMediaImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TypeName",
                table: "CustomerMediaImages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediaID",
                table: "CustomerArticles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "CustomerArticles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TypeName",
                table: "CustomerArticles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediaID",
                table: "CustomerArticleGroups",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "CustomerArticleGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TypeName",
                table: "CustomerArticleGroups",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "CustomerMediaVoices");

            migrationBuilder.DropColumn(
                name: "TypeName",
                table: "CustomerMediaVoices");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "CustomerMediaVideos");

            migrationBuilder.DropColumn(
                name: "TypeName",
                table: "CustomerMediaVideos");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "CustomerMediaImages");

            migrationBuilder.DropColumn(
                name: "TypeName",
                table: "CustomerMediaImages");

            migrationBuilder.DropColumn(
                name: "MediaID",
                table: "CustomerArticles");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "CustomerArticles");

            migrationBuilder.DropColumn(
                name: "TypeName",
                table: "CustomerArticles");

            migrationBuilder.DropColumn(
                name: "MediaID",
                table: "CustomerArticleGroups");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "CustomerArticleGroups");

            migrationBuilder.DropColumn(
                name: "TypeName",
                table: "CustomerArticleGroups");
        }
    }
}
