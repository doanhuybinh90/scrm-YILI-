using Microsoft.EntityFrameworkCore.Migrations;

namespace Pb.Wechat.Migrations
{
    public partial class changempmessagefield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupNames",
                table: "MpMessages");

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "MpMessages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "MpMessages");

            migrationBuilder.AddColumn<string>(
                name: "GroupNames",
                table: "MpMessages",
                nullable: true);
        }
    }
}
