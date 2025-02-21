
using Microsoft.EntityFrameworkCore;

namespace L01_2022ML601.Models
{
    public class restauranteDBContext : DbContext
    {
        public restauranteDBContext(DbContextOptions<restauranteDBContext> options) : base(options)
        {

        }
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Motoristas> Motoristas { get; set; }
        public DbSet<Pedidos> Pedidos { get; set; }
        public DbSet<Platos> Platos { get; set; }
    }
}
