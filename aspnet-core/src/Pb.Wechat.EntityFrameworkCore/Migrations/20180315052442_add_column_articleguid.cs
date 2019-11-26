using Microsoft.EntityFrameworkCore.Migrations;

namespace Pb.Wechat.Migrations
{
    public partial class add_column_articleguid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArticleGrid",
                table: "MpMediaArticles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticleGrid",
                table: "MpArticleInsideImages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticleGrid",
                table: "MpMediaArticles");

            migrationBuilder.DropColumn(
                name: "ArticleGrid",
                table: "MpArticleInsideImages");
        }
    }
}
