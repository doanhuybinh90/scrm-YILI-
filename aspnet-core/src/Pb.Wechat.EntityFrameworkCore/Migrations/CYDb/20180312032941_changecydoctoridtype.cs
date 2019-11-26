using Microsoft.EntityFrameworkCore.Migrations;

namespace Pb.Wechat.Migrations.CYDb
{
    public partial class changecydoctoridtype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CYDoctors_CYId",
                table: "CYDoctors");

            migrationBuilder.AddColumn<string>(
                name: "BabyName",
                table: "CYProblems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "CYProblems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telephone",
                table: "CYProblems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "CYProblems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctorName",
                table: "CYProblemContents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CYId",
                table: "CYDoctors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BabyName",
                table: "CYProblems");

            migrationBuilder.DropColumn(
                name: "NickName",
                table: "CYProblems");

            migrationBuilder.DropColumn(
                name: "Telephone",
                table: "CYProblems");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "CYProblems");

            migrationBuilder.DropColumn(
                name: "DoctorName",
                table: "CYProblemContents");

            migrationBuilder.AlterColumn<int>(
                name: "CYId",
                table: "CYDoctors",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CYDoctors_CYId",
                table: "CYDoctors",
                column: "CYId");
        }
    }
}
