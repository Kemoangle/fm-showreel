using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Showreel.Migrations
{
    /// <inheritdoc />
    public partial class removeduration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "duration",
                table: "playlist");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "duration",
                table: "playlist",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                collation: "utf8mb3_general_ci")
                .Annotation("MySql:CharSet", "utf8mb3");
        }
    }
}
