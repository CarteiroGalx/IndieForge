using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IndieForge.Migrations
{
    /// <inheritdoc />
    public partial class EditTotalArrecadadoETotalContribuicoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalArrecadado",
                table: "Projects",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TotalContribuicoes",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalArrecadado",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TotalContribuicoes",
                table: "Projects");
        }
    }
}
