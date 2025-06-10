using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using StoreApp.Models;
namespace StoreApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Return> Returns { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlServer("Server=localhost,1433;Database=StoreAppDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<OrderProduct>().HasKey(k => new { k.OrderId, k.ProductId });
            builder.Entity<OrderProduct>().HasOne(k => k.Order).WithMany(o => o.OrderProducts).HasForeignKey(k => k.OrderId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<OrderProduct>().HasOne(k => k.Product).WithMany(p => p.OrderProducts).HasForeignKey(k => k.ProductId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Order>().HasOne(o => o.User).WithMany().HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Cascade);

            //// Relazione molti-a-uno: Order → Return
            //builder.Entity<Return>()
            //    .HasOne(r => r.Order)
            //    //.WithMany(o => o.Returns)
            //    .WithMany()
            //    .HasForeignKey(r => r.OrderId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //// Relazione molti-a-uno: ApplicationUser → Return
            //builder.Entity<Return>()
            //    .HasOne(r => r.User)
            //    .WithMany()
            //    .HasForeignKey(r => r.UserId)
            //    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}