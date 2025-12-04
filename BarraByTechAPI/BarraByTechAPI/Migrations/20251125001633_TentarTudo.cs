using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarraByTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class TentarTudo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ValorProdutos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ValorProdutos",
                columns: table => new
                {
                    ValorProdutoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Boleto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cartao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Promocao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Vista = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValorProdutos", x => x.ValorProdutoId);
                    table.ForeignKey(
                        name: "FK_ValorProdutos_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "ProdutoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ValorProdutos_ProdutoId",
                table: "ValorProdutos",
                column: "ProdutoId");
        }
    }
}
