using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Data.Models
{
    public class BiddingFare
    {
        public int BiddingFareId { get; set; }
        public decimal BiddingCost { get; set; }
        public decimal BiddingRange { get; set; }
    }
}
