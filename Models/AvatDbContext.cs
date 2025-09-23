using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace avatCo.Models
{
    public class AvatDbContext : DbContext
    {
        public AvatDbContext(DbContextOptions<AvatDbContext> options)
        : base(options) { }

        public DbSet<TestEntity> TestEntities { get; set; }
        public DbSet<Product> Products { get; set; }
        // User & order flow
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: Configure relationships explicitly
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // or Cascade if you prefer
        }

    }
}