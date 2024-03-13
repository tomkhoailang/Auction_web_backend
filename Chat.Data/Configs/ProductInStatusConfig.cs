using Chat.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Data.Configs
{
    public class ProductInStatusConfig : IEntityTypeConfiguration<ProductInStatus>
    {
        public void Configure(EntityTypeBuilder<ProductInStatus> builder)
        {
            builder.HasKey(pis => pis.ProductInStatusID);
            builder.Property(pis => pis.ProductInStatusID).ValueGeneratedOnAdd();
            builder.Property(pis => pis.Timestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.HasOne(pis => pis.Product).WithMany(p => p.ProductInStatuses).HasForeignKey(pis => pis.ProductId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(pis => pis.ProductStatus).WithMany(p => p.ProductInStatuses).HasForeignKey(pis => pis.ProductStatusId).OnDelete(DeleteBehavior.NoAction);


        }
    }
}
