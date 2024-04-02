using Microsoft.EntityFrameworkCore;
using Shop.Domain;

namespace Shop.Infrastructure
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }


        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
           this.Database.EnsureCreated();   
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SeedProducts(modelBuilder);
            modelBuilder.Entity<Order>().OwnsOne<Amount>(p=>p.Amount);
            modelBuilder.Entity<Order>().OwnsMany<OrderProduct>(o => o.Products).OwnsOne<ProductItem>(o=>o.Replaced_with);
        }
        private void SeedProducts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { ID=123, Name="Ketchup", Price="0.45" },
                new Product { ID = 456, Name = "Beer", Price = "2.33" },
                new Product { ID = 879, Name = "Õllesnäkk", Price = "0.42" },
                new Product { ID = 999, Name = "75\" OLED TV", Price = "1333.37" }
                );
        }
    }
}
