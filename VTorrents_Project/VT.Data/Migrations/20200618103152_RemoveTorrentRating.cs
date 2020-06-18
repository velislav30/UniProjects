using Microsoft.EntityFrameworkCore.Migrations;

namespace VT.Data.Migrations
{
    public partial class RemoveTorrentRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Torrents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Rating",
                table: "Torrents",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
