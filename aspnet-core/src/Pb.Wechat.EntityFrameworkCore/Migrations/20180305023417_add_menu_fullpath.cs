using Microsoft.EntityFrameworkCore.Migrations;

namespace Pb.Wechat.Migrations
{
    public partial class add_menu_fullpath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EMail",
                table: "MpUserMembers");

            migrationBuilder.AlterColumn<string>(
                name: "OpenID",
                table: "MpUserMembers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "MenuFullPath",
                table: "MpMenus",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuFullPath",
                table: "MpMenus");

            migrationBuilder.AlterColumn<string>(
                name: "OpenID",
                table: "MpUserMembers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EMail",
                table: "MpUserMembers",
                nullable: true);
        }
    }
}
