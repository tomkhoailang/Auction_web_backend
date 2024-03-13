using Chat.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Data.Configs
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.ProductId);
            builder.Property(p => p.ProductId).ValueGeneratedOnAdd();
            builder.Property(p => p.InitialPrice).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(p => p.MinimumStep).HasColumnType("decimal(18,2)").IsRequired();
            builder.HasOne(p => p.Seller).WithMany(s => s.SellingProducts).HasForeignKey(p => p.SellerId).OnDelete(DeleteBehavior.NoAction).IsRequired();


        }
    }
}
