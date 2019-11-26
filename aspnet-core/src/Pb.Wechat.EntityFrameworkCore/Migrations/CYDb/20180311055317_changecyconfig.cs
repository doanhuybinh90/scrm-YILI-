using Microsoft.EntityFrameworkCore.Migrations;

namespace Pb.Wechat.Migrations.CYDb
{
    public partial class changecyconfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateProblemPreText",
                table: "CYConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreateProblemPreTextLength",
                table: "CYConfigs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NoOperationCloseMinute",
                table: "CYConfigs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateProblemPreText",
                table: "CYConfigs");

            migrationBuilder.DropColumn(
                name: "CreateProblemPreTextLength",
                table: "CYConfigs");

            migrationBuilder.DropColumn(
                name: "NoOperationCloseMinute",
                table: "CYConfigs");
        }
    }
}
