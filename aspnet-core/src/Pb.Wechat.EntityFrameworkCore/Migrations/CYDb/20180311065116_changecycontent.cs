using Microsoft.EntityFrameworkCore.Migrations;

namespace Pb.Wechat.Migrations.CYDb
{
    public partial class changecycontent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ATime",
                table: "CYProblems");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "CYProblemContents",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ATime",
                table: "CYProblems",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "CYProblemContents",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
