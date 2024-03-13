using Chat.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Data.Configs
{
    public class BiddingConfig : IEntityTypeConfiguration<Bidding>
    {
        public void Configure(EntityTypeBuilder<Bidding> builder)
        {
            builder.HasKey(b => b.BiddingId);
            builder.Property(b => b.BiddingId).ValueGeneratedOnAdd();
            builder.Property(b => b.Timestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(b => b.BiddingAmount).HasColumnType("decimal(18,2)").IsRequired();
            builder.HasOne(b => b.BiddingUser).WithMany(bu => bu.Biddings).HasForeignKey(b => b.BiddingUserId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(b => b.Product).WithMany(p => p.Biddings).HasForeignKey(p => p.ProductId).IsRequired().OnDelete(DeleteBehavior.NoAction);

        }
    }
}
