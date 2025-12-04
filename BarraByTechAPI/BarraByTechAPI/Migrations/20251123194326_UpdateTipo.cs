using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarraByTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTipo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoriaNome",
                table: "Produtos");

            migrationBuilder.RenameColumn(
                name: "TipoId",
                table: "Produtos",
                newName: "TipoMarcaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TipoMarcaId",
                table: "Produtos",
                newName: "TipoId");

            migrationBuilder.AddColumn<string>(
                name: "CategoriaNome",
                table: "Produtos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
