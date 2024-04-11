using Chat.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Data.Configs
{
    public class ChatRoomUserConfig : IEntityTypeConfiguration<ChatRoomUser>
    {
        public void Configure(EntityTypeBuilder<ChatRoomUser> builder)
        {
            builder.HasKey(p => new { p.UserId, p.ChatRoomId });
            builder.HasOne(p => p.User).WithMany(u => u.JoinedChatRooms).HasForeignKey(u => u.UserId);
            builder.HasOne(p => p.ChatRoom).WithMany(cr => cr.Users).HasForeignKey(u => u.ChatRoomId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
