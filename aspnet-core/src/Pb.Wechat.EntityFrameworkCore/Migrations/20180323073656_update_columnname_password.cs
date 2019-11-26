using Microsoft.EntityFrameworkCore.Migrations;

namespace Pb.Wechat.Migrations
{
    public partial class update_columnname_password : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassWord",
                table: "CustomerServiceOnlines");

            migrationBuilder.AddColumn<string>(
                name: "KfPassWord",
                table: "CustomerServiceOnlines",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KfPassWord",
                table: "CustomerServiceOnlines");

            migrationBuilder.AddColumn<string>(
                name: "PassWord",
                table: "CustomerServiceOnlines",
                nullable: true);
        }
    }
}
