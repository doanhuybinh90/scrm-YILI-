using Microsoft.EntityFrameworkCore.Migrations;

namespace Pb.Wechat.Migrations
{
    public partial class addcolumn_customer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreKfAccount",
                table: "CustomerServiceOnlines",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreKfAccount",
                table: "CustomerServiceOnlines");
        }
    }
}
