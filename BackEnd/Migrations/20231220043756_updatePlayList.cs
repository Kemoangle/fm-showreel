using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Showreel.Migrations
{
    /// <inheritdoc />
    public partial class updatePlayList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "playlist",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "playlist");
        }
    }
}
