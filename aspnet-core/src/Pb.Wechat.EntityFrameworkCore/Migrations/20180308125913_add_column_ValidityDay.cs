using Microsoft.EntityFrameworkCore.Migrations;

namespace Pb.Wechat.Migrations
{
    public partial class add_column_ValidityDay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ValidityDay",
                table: "MpChannels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidityDay",
                table: "MpChannels");
        }
    }
}
