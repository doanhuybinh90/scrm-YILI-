using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pb.Wechat.Migrations.CYDb
{
    public partial class addCYConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CYConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CloseProblemText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateProblemText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoOperationMinute = table.Column<int>(type: "int", nullable: false),
                    NoOperationText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CYConfigs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CYConfigs");
        }
    }
}
