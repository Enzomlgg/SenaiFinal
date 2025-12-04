using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarraByTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoMarcaId",
                table: "Produtos");

            migrationBuilder.AddColumn<decimal>(
                name: "Boleto",
                table: "Produtos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Cartao",
                table: "Produtos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Promocao",
                table: "Produtos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Vista",
                table: "Produtos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Boleto",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Cartao",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Promocao",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Vista",
                table: "Produtos");

            migrationBuilder.AddColumn<Guid>(
                name: "TipoMarcaId",
                table: "Produtos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
