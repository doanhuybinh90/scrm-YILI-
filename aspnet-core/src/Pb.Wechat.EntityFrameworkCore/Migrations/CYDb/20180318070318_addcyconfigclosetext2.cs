using Microsoft.EntityFrameworkCore.Migrations;

namespace Pb.Wechat.Migrations.CYDb
{
    public partial class addcyconfigclosetext2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CloseNotice",
                table: "CYProblems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CloseProblemPreText",
                table: "CYConfigs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseNotice",
                table: "CYProblems");

            migrationBuilder.DropColumn(
                name: "CloseProblemPreText",
                table: "CYConfigs");
        }
    }
}
