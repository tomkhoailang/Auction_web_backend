using Chat.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Data.Configs
{
    public class BiddingFareConfig : IEntityTypeConfiguration<BiddingFare>
    {
        public void Configure(EntityTypeBuilder<BiddingFare> builder)
        {
            builder.HasKey(b => b.BiddingFareId);
            builder.Property(b => b.BiddingCost).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(b => b.BiddingRange).HasColumnType("decimal(18,2)").IsRequired();
        }
    }
}
