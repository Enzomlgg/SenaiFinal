using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarraByTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixEnderecoErro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnderecosCliente_Clientes_ClienteUserId",
                table: "EnderecosCliente");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClienteUserId",
                table: "EnderecosCliente",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_EnderecosCliente_Clientes_ClienteUserId",
                table: "EnderecosCliente",
                column: "ClienteUserId",
                principalTable: "Clientes",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnderecosCliente_Clientes_ClienteUserId",
                table: "EnderecosCliente");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClienteUserId",
                table: "EnderecosCliente",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EnderecosCliente_Clientes_ClienteUserId",
                table: "EnderecosCliente",
                column: "ClienteUserId",
                principalTable: "Clientes",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
