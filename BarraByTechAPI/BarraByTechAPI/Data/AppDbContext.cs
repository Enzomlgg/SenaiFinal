using BarraByTechAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BarraByTechAPI.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<EnderecoCliente> EnderecosCliente { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedidos { get; set; }
        public DbSet<Compras> Compras { get; set; }
        public DbSet<Marcas> Marcas { get; set; }
        public DbSet<ItensFavoritos> ItensFavoritos { get; set; }
        public DbSet<TiposMarca> TiposMarcas { get; set; }
        public DbSet<ItemCarrinho> ItemCarrinho { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EnderecoCliente>()
                .HasOne(e => e.Cliente)
                .WithMany()
                .HasForeignKey(e => e.UserId);
        }
    }
}
