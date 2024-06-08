using Microsoft.EntityFrameworkCore;
using AuctionAPI.Models;

namespace AuctionAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
    }
}
