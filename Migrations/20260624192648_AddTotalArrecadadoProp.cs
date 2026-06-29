using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IndieForge.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalArrecadadoProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalArrecadado",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalArrecadado",
                table: "Projects");
        }
    }
}
