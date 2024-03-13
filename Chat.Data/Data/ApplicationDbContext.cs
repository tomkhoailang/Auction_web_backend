using Chat.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Chat.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Bidding> Biddings { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductStatus> ProductStatuses { get; set; }
        public DbSet<ChatRoomProduct> ChatRoomProducts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }






        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            SeedRoles(builder);
        }
        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" }
                );
            builder.Entity<ProductStatus>().HasData(
                new ProductStatus() { ProductStatusId = 1, ProductStatusName = "Waiting to accept" },
                new ProductStatus() { ProductStatusId = 2, ProductStatusName = "Your product has been registered successfully" },
                new ProductStatus() { ProductStatusId = 3, ProductStatusName = "Auction in Progress" },
                new ProductStatus() { ProductStatusId = 4, ProductStatusName = "Sold" },
                new ProductStatus() { ProductStatusId = 5, ProductStatusName = "Expired" }
                );
        }
    }
}
