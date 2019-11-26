using Microsoft.EntityFrameworkCore.Migrations;

namespace Pb.Wechat.Migrations
{
    public partial class addusermemberfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupIDs",
                table: "MpMessages");

            migrationBuilder.AddColumn<string>(
                name: "ServiceShopCode",
                table: "MpUserMembers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupID",
                table: "MpMessages",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceShopCode",
                table: "MpUserMembers");

            migrationBuilder.DropColumn(
                name: "GroupID",
                table: "MpMessages");

            migrationBuilder.AddColumn<string>(
                name: "GroupIDs",
                table: "MpMessages",
                nullable: true);
        }
    }
}
