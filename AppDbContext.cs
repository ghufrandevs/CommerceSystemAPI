using CommerceSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CommerceSystemAPI
{
    public class AppDbContext :DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = CommerceWebApiProject; Integrated Security = true; TrustServerCertificate = True ");
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderProducts> OrderProductss { get; set; }

        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderProducts>()
                .HasKey(op => new { op.OrderId, op.ProductId });
        }
    }
}
