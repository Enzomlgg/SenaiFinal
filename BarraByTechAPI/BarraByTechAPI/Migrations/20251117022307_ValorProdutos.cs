using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarraByTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class ValorProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ValorProdutos",
                columns: table => new
                {
                    ValorProdutoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Boleto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cartao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Vista = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Promocao = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValorProdutos", x => x.ValorProdutoId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ValorProdutos");
        }
    }
}
