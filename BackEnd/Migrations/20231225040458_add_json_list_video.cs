using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Showreel.Migrations
{
    /// <inheritdoc />
    public partial class add_json_list_video : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JsonListVideo",
                table: "playlist",
                type: "longtext",
                nullable: true,
                collation: "utf8mb3_general_ci")
                .Annotation("MySql:CharSet", "utf8mb3");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JsonListVideo",
                table: "playlist");
        }
    }
}
