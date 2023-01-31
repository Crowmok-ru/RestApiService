using RestApiService.Model;
using Microsoft.EntityFrameworkCore;

namespace RestApiService.Context
{
    public class EFDbContext : DbContext
    {
        public EFDbContext(DbContextOptions<EFDbContext> options) 
            : base(options) { }
        public DbSet<Client> Client { get; set; }
        public DbSet<OrderPositions> OrderPositions { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<ProductType> ProductType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }

    
}
