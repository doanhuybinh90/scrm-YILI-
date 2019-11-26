using Microsoft.EntityFrameworkCore.Migrations;

namespace Pb.Wechat.Migrations
{
    public partial class update_table_channel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMcChannel",
                table: "MpChannels",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMcChannel",
                table: "MpChannels");
        }
    }
}
