using Chat.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Data.Configs
{
    public class ChatRoomConfig : IEntityTypeConfiguration<ChatRoom>
    {
        public void Configure(EntityTypeBuilder<ChatRoom> builder)
        {
            builder.HasKey(cr => cr.ChatRoomId);
            builder.Property(cr => cr.ChatRoomId).ValueGeneratedOnAdd();
            builder.HasOne(cr => cr.HostUser).WithMany(hu => hu.HostRooms).HasForeignKey(cr => cr.HostUserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(cr => cr.Messages).WithOne(m => m.ChatRoom).HasForeignKey(m => m.ChatRoomId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(cr => cr.Users).WithMany(u => u.JoinedChatRooms).UsingEntity<Dictionary<string, object>>("ChatRoomUser",
                j => j.HasOne<ApplicationUser>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.NoAction),
                j => j.HasOne<ChatRoom>().WithMany().HasForeignKey("ChatRoomId").OnDelete(DeleteBehavior.NoAction),
                j =>
                {
                    j.HasKey("UserId", "ChatRoomId");
                }
                );
        }
    }
}
