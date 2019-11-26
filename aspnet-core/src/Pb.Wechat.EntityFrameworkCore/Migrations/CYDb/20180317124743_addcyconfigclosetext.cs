using Microsoft.EntityFrameworkCore.Migrations;

namespace Pb.Wechat.Migrations.CYDb
{
    public partial class addcyconfigclosetext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NoOperationCloseText",
                table: "CYConfigs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoOperationCloseText",
                table: "CYConfigs");
        }
    }
}
