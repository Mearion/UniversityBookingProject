namespace AuctionAPI.Models
{
    public class AuctionInfo
    {
        public string Name { get; set; }
        public int UID { get; set; }
        public string Email { get; set; }
        public double BidAmount { get; set; }
        public Boolean isHighestBidder { get; set; }
    }
}
