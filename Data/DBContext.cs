using Microsoft.EntityFrameworkCore;
using PrimerCrudWebAPI.Models;

namespace PrimerCrudWebAPI.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //cada nombre de producto debe ser único
            modelBuilder.Entity<Producto>().HasIndex(p => p.Nombre).IsUnique();


            //cada nombre de categroia debe ser unico
            modelBuilder.Entity<Categoria>().HasIndex(c => c.Nombre).IsUnique();
        }
    }
}
