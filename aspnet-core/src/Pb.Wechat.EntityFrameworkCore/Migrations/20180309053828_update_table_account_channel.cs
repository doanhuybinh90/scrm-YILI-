using Microsoft.EntityFrameworkCore.Migrations;

namespace Pb.Wechat.Migrations
{
    public partial class update_table_account_channel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaID",
                table: "MpChannels");

            migrationBuilder.AddColumn<int>(
                name: "MemberID",
                table: "MpFans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Ticket",
                table: "MpChannels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WxAccount",
                table: "MpAccounts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberID",
                table: "MpFans");

            migrationBuilder.DropColumn(
                name: "Ticket",
                table: "MpChannels");

            migrationBuilder.DropColumn(
                name: "WxAccount",
                table: "MpAccounts");

            migrationBuilder.AddColumn<string>(
                name: "MediaID",
                table: "MpChannels",
                nullable: true);
        }
    }
}
