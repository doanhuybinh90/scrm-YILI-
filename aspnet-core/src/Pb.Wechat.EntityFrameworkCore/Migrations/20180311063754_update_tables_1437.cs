using Microsoft.EntityFrameworkCore.Migrations;

namespace Pb.Wechat.Migrations
{
    public partial class update_tables_1437 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChannelType",
                table: "MpFans",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChannelType",
                table: "MpFans");
        }
    }
}
