using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarraByTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class ClienteCpf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnderecosCliente_Clientes_ClienteUserId",
                table: "EnderecosCliente");

            migrationBuilder.DropIndex(
                name: "IX_EnderecosCliente_ClienteUserId",
                table: "EnderecosCliente");

            migrationBuilder.DropColumn(
                name: "ClienteUserId",
                table: "EnderecosCliente");

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_EnderecosCliente_UserId",
                table: "EnderecosCliente",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnderecosCliente_Clientes_UserId",
                table: "EnderecosCliente",
                column: "UserId",
                principalTable: "Clientes",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnderecosCliente_Clientes_UserId",
                table: "EnderecosCliente");

            migrationBuilder.DropIndex(
                name: "IX_EnderecosCliente_UserId",
                table: "EnderecosCliente");

            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "Clientes");

            migrationBuilder.AddColumn<Guid>(
                name: "ClienteUserId",
                table: "EnderecosCliente",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnderecosCliente_ClienteUserId",
                table: "EnderecosCliente",
                column: "ClienteUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnderecosCliente_Clientes_ClienteUserId",
                table: "EnderecosCliente",
                column: "ClienteUserId",
                principalTable: "Clientes",
                principalColumn: "UserId");
        }
    }
}
