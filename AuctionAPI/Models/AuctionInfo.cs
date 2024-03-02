using System.ComponentModel.DataAnnotations;

namespace AuctionAPI.Models
{
    public class AuctionInfo
    {
        [Key]
        public required string Name { get; set; }
        public string UserID { get; set; }
        public required string Email { get; set; }
        public double BidAmount { get; set; }
        public Boolean isHighestBidder { get; set; }
    }
}
