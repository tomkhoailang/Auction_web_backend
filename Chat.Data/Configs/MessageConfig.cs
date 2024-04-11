using Chat.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Data.Configs
{
    public class MessageConfig : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.MessageId);
            builder.Property(m => m.MessageId).ValueGeneratedOnAdd();
            builder.HasOne(m => m.Sender).WithMany(s => s.Messages).HasForeignKey(s => s.SenderId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(m => m.ChatRoom).WithMany(cr => cr.Messages).HasForeignKey(cr => cr.ChatRoomId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
