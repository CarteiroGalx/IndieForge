using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IndieForge.Migrations
{
    /// <inheritdoc />
    public partial class ReformaClasseProjeto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalArrecadado",
                table: "Projects",
                newName: "DataCriacao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataCriacao",
                table: "Projects",
                newName: "TotalArrecadado");
        }
    }
}
