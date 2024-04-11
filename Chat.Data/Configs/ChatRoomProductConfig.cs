using Chat.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Data.Configs
{
    public class ChatRoomProductConfig : IEntityTypeConfiguration<ChatRoomProduct>
    {
        public void Configure(EntityTypeBuilder<ChatRoomProduct> builder)
        {
            builder.HasKey(crp => crp.ChatRoomProductId);
            builder.Property(crp => crp.ChatRoomProductId).ValueGeneratedOnAdd();
            builder.HasOne(crp => crp.Product).WithMany(p => p.ChatRoomProducts).HasForeignKey(crp => crp.ProductId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(crp => crp.ChatRoom).WithMany(cr => cr.ChatRoomProducts).HasForeignKey(crp => crp.ChatRoomId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
