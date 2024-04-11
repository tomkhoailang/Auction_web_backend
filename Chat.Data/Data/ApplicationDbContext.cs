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
        public DbSet<ProductInStatus> ProductInStatuses { get; set; }
        public DbSet<ChatRoomProduct> ChatRoomProducts { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ChatRoomUser> ChatRoomUsers { get; set; }
        public DbSet<BiddingFare> BiddingFares { get; set; }


        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            var entityTypes = builder.Model.GetEntityTypes();

            builder.Entity<Bidding>().HasQueryFilter(x => x.IsDeleted == false);
            builder.Entity<ChatRoom>().HasQueryFilter(x => x.IsDeleted == false);
            builder.Entity<ChatRoomProduct>().HasQueryFilter(x => x.IsDeleted == false);
            builder.Entity<ChatRoomUser>().HasQueryFilter(x => x.IsDeleted == false);
            builder.Entity<Message>().HasQueryFilter(x => x.IsDeleted == false);
            builder.Entity<Product>().HasQueryFilter(x => x.IsDeleted == false);
            builder.Entity<ProductImage>().HasQueryFilter(x => x.IsDeleted == false);
            builder.Entity<ProductInStatus>().HasQueryFilter(x => x.IsDeleted == false);



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
                new ProductStatus() { ProductStatusId = 2, ProductStatusName = "Your product has been registered successfully" }
                );

            builder.Entity<BiddingFare>().HasData(
                new BiddingFare() { BiddingFareId = 1, BiddingRange = 20000000, BiddingCost = 4  },
                new BiddingFare() { BiddingFareId = 2, BiddingRange = 50000000, BiddingCost = 6 },
                new BiddingFare() { BiddingFareId = 3, BiddingRange = 100000000, BiddingCost = 9},
                new BiddingFare() { BiddingFareId = 4, BiddingRange = 100000000000, BiddingCost =  11}

                );
        }
    }
}
