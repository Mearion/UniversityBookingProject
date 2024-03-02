using AuctionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI.Data
{
    public class AuctionContext : DbContext
    {
        public DbSet<AuctionInfo> AuctionInfos { get; set; }
        public AuctionContext(DbContextOptions<AuctionContext> options) : base(options)
        {

        }
    }
}
